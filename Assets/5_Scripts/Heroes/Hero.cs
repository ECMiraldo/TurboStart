using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
public class HeroStats
{
    [field: SerializeField]
    public SerializedDictionary<CombatStats, Attribute> stats { get; set; } = new();

    //[field: SerializeField] public List<IDamageModifier> damageModifiers = new();
    [field: SerializeField] public Element element { get; protected set; }

    public HeroStats()
    {
        foreach (CombatStats stat in Enum.GetValues(typeof(CombatStats)))
        {
            var attribute = new Attribute(0);
            stats.Add(stat, attribute);
        }
    }

    public Attribute GetStat(CombatStats attr)
    {
        if (stats.ContainsKey(attr)) return stats[attr];
        return null;
    }

    public float GetValue(CombatStats attr)
    {
        if (stats.ContainsKey(attr)) return stats[attr].FinalValue;
        return 0;
    }

    protected void ApplyFighterDataModifiers(FighterData data, string source)
    {
        var mappings = new Dictionary<CombatStats, float>
    {
        //{ CombatStats.MATK,        data.Attack },
        //{ CombatStats.ATTACK_SPEED,  data.AttackSpeed },
        //{ CombatStats.MAGIC_ATTACK,  data.MagicAttack },
        //{ CombatStats.ACCURACY,      data.Accuracy },
        //{ CombatStats.CRIT_CHANCE,   data.CritChance },
        //{ CombatStats.MANA,          data.Mana },
        //{ CombatStats.MANA_REGEN,    data.ManaRegen },
        //{ CombatStats.HEALTH,        data.Health },
        //{ CombatStats.HEALTH_REGEN,  data.HealthRegen },
        //{ CombatStats.DEFENSE,       data.Defense },
        //{ CombatStats.MAGIC_DEFENSE, data.MagicDefense },
        //{ CombatStats.EVASION,       data.Evasion },
        //{ CombatStats.MOVE_SPEED,    data.MoveSpeed },
    };

        foreach (var (attribute, value) in mappings)
        {
            AddModifier(attribute, value, source);
        }
    }

    private void AddModifier(CombatStats stat, float value, string source)
    {
        if (!stats.TryGetValue(stat, out var attribute))
            return;

        var mod = new FixedAttributeModifier(value, StatModType.Flat, source);
        attribute.AddModifier(mod);
    }
    
}



[Serializable]
public class Hero
{
    [JsonProperty][field: SerializeField] public string Name { get; private set; }
    [JsonProperty][field: SerializeField] public int Id { get; private set; }
    [JsonProperty][field: SerializeField] public HeroAction Action { get; protected set; }
    [JsonProperty][field: SerializeField] public string spriteName { get; private set; }
    [JsonProperty][field: SerializeField] public List<Attribute> MainAttibutes { get; private set; }

    [JsonIgnore] private Sprite _sprite;
    [JsonIgnore]
    public Sprite Sprite
    {
        get
        {
            if (_sprite != null) return _sprite;
            else _sprite = Resources.Load<Sprite>($"{Constants.HERO_SPRITES_ADDRESS}/{spriteName}");
            return _sprite;
        }
    }


    [JsonConstructor]
    public Hero(int id, string name, string spriteAddress, HeroAction action)
    {
        this.Name = name;
        this.Id = id;
        this.spriteName = spriteAddress;
        this.Action = action;
    }


    public Hero(int id, string name, string spriteAddress)
    {
        this.Name = name;
        this.Id = id;
        this.spriteName = spriteAddress; 
    }

    public void AssignAction(HeroAction action)
    {
        this.Action = action;
        action.OnActionFinished += OnActionFinished;
        
    }
    protected virtual void OnActionFinished(HeroAction finishedAction)
    {
        Action.OnActionFinished -= OnActionFinished;
        Action = null;
       
    }
}


