using AYellowpaper.SerializedCollections;
using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;

public abstract class UnitDataSO : IDScriptableObject
{
    [Header("General")]
    public GameObject prefab;
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; } = new Vector2Int[] { Vector2Int.zero };
    [field: SerializeField] public UnitArchetypeSO archetype { get; private set; }
    [field: SerializeField] public SkillSO skill { get; private set; }
    [SerializeField] public List<StatGrowthRule> statGrowthRules = new();

    [Header("Stats")]
    public int attack;
    public int magicAttack;
    public float attackSpeed;
    public int attackRange;
    public int accuracy;
    public float critChance;
    public int health;
    public int focus;
    public int defense;
    public int magicDefense;
    public int evasion;

    [Header("Balancing")]

    [SerializeField,ReadOnly] private float offensiveRating;
    [SerializeField,ReadOnly] private float defensiveRating;
    [SerializeField,ReadOnly] private float magicRating;
    [SerializeField,ReadOnly] private float weightedRating;
    [SerializeField, ReadOnly] private float perLevelGrow;

    public SerializedDictionary<UnitStat, Attribute> GetStats(int round)
    {
        return new()
        {
            { UnitStat.Health, new Attribute(GetScaledStatValue(UnitStat.Health, health, round)) },
            { UnitStat.Focus, new Attribute(GetScaledStatValue(UnitStat.Focus, focus, round)) },
            { UnitStat.Attack, new Attribute(GetScaledStatValue(UnitStat.Attack, attack, round)) },
            { UnitStat.MagicAttack, new Attribute(GetScaledStatValue(UnitStat.MagicAttack, magicAttack, round)) },
            { UnitStat.AttackSpeed, new Attribute(GetScaledStatValue(UnitStat.AttackSpeed, attackSpeed, round)) },
            { UnitStat.AttackRange, new Attribute(GetScaledStatValue(UnitStat.AttackRange, attackRange, round)) },
            { UnitStat.Accuracy, new Attribute(GetScaledStatValue(UnitStat.Accuracy, accuracy, round)) },
            { UnitStat.CritChance, new Attribute(GetScaledStatValue(UnitStat.CritChance, critChance, round)) },
            { UnitStat.Defense, new Attribute(GetScaledStatValue(UnitStat.Defense, defense, round)) },
            { UnitStat.MagicDefense, new Attribute(GetScaledStatValue(UnitStat.MagicDefense, magicDefense, round)) },
            { UnitStat.Evasion, new Attribute(GetScaledStatValue(UnitStat.Evasion, evasion, round)) },
            { UnitStat.FocusOnDamageTaken, new Attribute(archetype.focusOnDamageTaken) },
            { UnitStat.FocusPerHit, new Attribute(archetype.focusPerHit) },
            { UnitStat.FocusPerSecond, new Attribute(archetype.focusPerSecond) },

        };
    }



    private float GetScaledStatValue(UnitStat stat, float baseValue, int round)
    {
        float flatGain = 0f;
        float percentGain = 0f;

        foreach (StatGrowthRule growthRule in GetGrowthRulesForStat(stat))
        {
            flatGain += growthRule.flatGainPerLevel * Mathf.Max(1, round);
            percentGain += growthRule.percentGainPerLevel * Mathf.Max(1, round);
        }

        return (baseValue + flatGain) * (1f + percentGain);
    }

    private IEnumerable<StatGrowthRule> GetGrowthRulesForStat(UnitStat stat)
    {
        return archetype.statGrowthRules
            .Where(x => x.stat == stat)
            .Concat(statGrowthRules.Where(x => x.stat == stat));
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        offensiveRating = CalculateOffensiveRating(
            GetBaseStat(UnitStat.Attack),
            GetBaseStat(UnitStat.AttackSpeed),
            GetBaseStat(UnitStat.CritChance),
            GetBaseStat(UnitStat.Accuracy)
        );
        defensiveRating = CalculateDefensiveRating(
            GetBaseStat(UnitStat.Health),
            GetBaseStat(UnitStat.Defense),
            GetBaseStat(UnitStat.MagicDefense)
        ) / 10f; //magic number
        magicRating = CalculateMagicRating(            
            GetBaseStat(UnitStat.MagicAttack),
            GetBaseStat(UnitStat.Focus),
            GetBaseStat(UnitStat.AttackSpeed)
        );
        weightedRating = 
            offensiveRating * archetype.offensiveRatingWeight +
            defensiveRating * archetype.defensiveRatingWeight +
            magicRating * archetype.magicRatingWeight;
        
        perLevelGrow = CalculateRatingPerLevel();
    }
#endif

  

    private int CalculateOffensiveRating(float attack, float atkSpeed, float crit, float accuracy)
    {
        float critMultiplier = 2f;

        float expectedCritMultiplier =
            1f + crit * (critMultiplier - 1f);

        //not counting accuracy or range yet
        float accuracyMultiplier =1;
        
            // accuracy / 100f;

        return Mathf.FloorToInt(
            attack
            * atkSpeed
            * expectedCritMultiplier
            * accuracyMultiplier);
    }

    private int CalculateDefensiveRating(float health, float defense, float magicDefense)
    {
        float physicalMitigation = Mathf.Pow(defense, 1.0f / 2.0f); //cubic root

        //cap damage reduction at 75%
        float magicMitigation = Mathf.Pow(magicDefense, 1.0f / 2.0f); //dubic root

        float physicalHP = health / ( 1 - physicalMitigation/ 100);
        float magicHP = health / ( 1 - magicMitigation / 100);

        return Mathf.FloorToInt((physicalHP + magicHP) * 0.5f);
    }

    private int CalculateMagicRating(float magic, float focus, float atkSpeed)
    {
        float focusPerSecond = archetype.focusPerSecond 
            + atkSpeed * archetype.focusPerHit 
            + 2 * archetype.focusOnDamageTaken;

        float secondsToCast = focus / focusPerSecond;

        float magicDamage = magic * (focus / 5); //magic number

        return Mathf.FloorToInt(magicDamage / secondsToCast);
    }

   public float CalculateRatingPerLevel()
    {
        var starterRating = CalculateRatingForlevel(1);

        var levelOneRating = CalculateRatingForlevel(50);

        //return Mathf.FloorToInt(((levelOneRating / starterRating) - 1f) * 100f);
        return ((float)levelOneRating / starterRating - 1f) * 100f;
    }

    private int CalculateRatingForlevel(int level)
    {
        var stats = GetStats(level);

        float offensive = CalculateOffensiveRating(
            stats[UnitStat.Attack].Value,
            stats[UnitStat.AttackSpeed].Value,
            stats[UnitStat.CritChance].Value,
            stats[UnitStat.Accuracy].Value
        );

        float defensive = CalculateDefensiveRating(
            stats[UnitStat.Health].Value,
            stats[UnitStat.Defense].Value,
            stats[UnitStat.MagicDefense].Value
        );

        float magic = CalculateMagicRating(
            stats[UnitStat.MagicAttack].Value,
            stats[UnitStat.Focus].Value,
            stats[UnitStat.AttackSpeed].Value
        );

        return Mathf.FloorToInt(
            offensive * archetype.offensiveRatingWeight +
            defensive * archetype.defensiveRatingWeight +
            magic * archetype.magicRatingWeight
        );
    }

    protected virtual float GetBaseStat(UnitStat stat)
    {
        return stat switch
        {
            UnitStat.Health => health,
            UnitStat.Focus => focus,
            UnitStat.Attack => attack,
            UnitStat.MagicAttack => magicAttack,
            UnitStat.AttackSpeed => attackSpeed,
            UnitStat.AttackRange => attackRange,
            UnitStat.Accuracy => accuracy,
            UnitStat.CritChance => critChance,
            UnitStat.Defense => defense,
            UnitStat.MagicDefense => magicDefense,
            UnitStat.Evasion => evasion,
            _ => 0
        };
    }


}
