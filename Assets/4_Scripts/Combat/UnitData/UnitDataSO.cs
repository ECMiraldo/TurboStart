using AYellowpaper.SerializedCollections;
using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;


public abstract class UnitDataSO : IDScriptableObject
{
    [Header("General")]
    public GameObject prefab;
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; } = new Vector2Int[] { Vector2Int.zero };
    [field: SerializeField] public UnitArchetypeSO archetype { get; private set; }
    [SerializeField] public List<StatGrowthRule> statGrowthRules = new();

    [Header("Stats")]
    public int attack;
    public int magicAttack;
    public float attackSpeed;
    public int attackRange;
    public int accuracy;
    public float critChance;
    public int health;
    public int mana;
    public int defense;
    public int magicDefense;
    public int evasion;

    [SerializeField, ReadOnly] private float starterRating;

    [SerializeField, ReadOnly]private float levelOneRating;

    [SerializeField, ReadOnly] private float perLevelRatingPercent;

    public SerializedDictionary<UnitStat, Attribute> GetStats(int round)
    {
        return new()
        {
            { UnitStat.Health, new Attribute(GetScaledStatValue(UnitStat.Health, health, round)) },
            { UnitStat.Focus, new Attribute(GetScaledStatValue(UnitStat.Focus, mana, round)) },
            { UnitStat.Attack, new Attribute(GetScaledStatValue(UnitStat.Attack, attack, round)) },
            { UnitStat.MagicAttack, new Attribute(GetScaledStatValue(UnitStat.MagicAttack, magicAttack, round)) },
            { UnitStat.AttackSpeed, new Attribute(GetScaledStatValue(UnitStat.AttackSpeed, attackSpeed, round)) },
            { UnitStat.AttackRange, new Attribute(GetScaledStatValue(UnitStat.AttackRange, attackRange, round)) },
            { UnitStat.Accuracy, new Attribute(GetScaledStatValue(UnitStat.Accuracy, accuracy, round)) },
            { UnitStat.CritChance, new Attribute(GetScaledStatValue(UnitStat.CritChance, critChance, round)) },
            { UnitStat.Defense, new Attribute(GetScaledStatValue(UnitStat.Defense, defense, round)) },
            { UnitStat.MagicDefense, new Attribute(GetScaledStatValue(UnitStat.MagicDefense, magicDefense, round)) },
            { UnitStat.Evasion, new Attribute(GetScaledStatValue(UnitStat.Evasion, evasion, round)) },
        };
    }

    private float GetScaledStatValue(UnitStat stat, float baseValue, int round)
    {
        float flatGain = 0f;
        float percentGain = 0f;

        foreach (StatGrowthRule growthRule in GetGrowthRulesForStat(stat))
        {
            if (growthRule == null)
                continue;

            flatGain += growthRule.flatGainPerLevel * Mathf.Max(1, round);
            percentGain += growthRule.percentGainPerLevel * Mathf.Max(1, round);
        }

        return (baseValue + flatGain) * (1f + percentGain);
    }

    private IEnumerable<StatGrowthRule> GetGrowthRulesForStat(UnitStat stat)
    {
        List<StatGrowthRule> growthRules = new();

        if (archetype != null && archetype.statGrowthRules != null)
        {
            foreach (StatGrowthRule growthRule in archetype.statGrowthRules)
            {
                if (growthRule != null && growthRule.stat == stat)
                    growthRules.Add(growthRule);
            }
        }

        if (statGrowthRules != null)
        {
            foreach (StatGrowthRule growthRule in statGrowthRules)
            {
                if (growthRule != null && growthRule.stat == stat)
                    growthRules.Add(growthRule);
            }
        }

        return growthRules;
    }
   public void CalculateRatings()
    {
        starterRating = CalculateRating(1);

        levelOneRating = CalculateRating(2);

        if (starterRating > 0)
        {
            perLevelRatingPercent =
                ((levelOneRating / starterRating) - 1f) * 100f;
        }
        else
        {
            perLevelRatingPercent = 0;
        }
    }
    private float CalculateRating(int level)
    {
        float rating = 0;

        foreach (UnitStat stat in Enum.GetValues(typeof(UnitStat)))
        {
            float value = GetBaseStat(stat);

            value = GetScaledStatValue(stat, value, level);

            float weight = GetRatingWeight(stat);

            rating += value * weight;
        }

        return rating;
    }
    private float GetRatingWeight(UnitStat stat)
    {
        StatGrowthRule rule = archetype?.GetRule(stat);

        return rule?.ratingWeight ?? 1f;
    }

    protected virtual float GetBaseStat(UnitStat stat)
    {
        return stat switch
        {
            UnitStat.Health => health,
            UnitStat.Focus => mana,
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculateRatings();
    }
    #endif
}
