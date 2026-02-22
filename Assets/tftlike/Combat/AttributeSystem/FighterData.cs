using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[Serializable]
public class FighterData : IUnitData
{

    [field: Header("Physical")]
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int MagicAttack { get; private set; }
    [field: SerializeField] public int Accuracy { get; private set; }
    [field: SerializeField] public float CritChance { get; private set; }


    [field: Header("Vitals")]
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }


    [field: Header("Defense")]
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int MagicDefense { get; private set; }
    [field: SerializeField] public int Evasion { get; private set; }


    [field: Header("Movement")]
    [field: SerializeField] public int initative { get; private set; }

    public SerializedDictionary<UnitStat, CharacterAttribute> GetStatsMap()
    {
        var map = new SerializedDictionary<UnitStat, CharacterAttribute>();

        // Physical
        map[UnitStat.Attack] = new CharacterAttribute(Attack);
        map[UnitStat.Accuracy] = new CharacterAttribute(Accuracy);
        map[UnitStat.CritChance] = new CharacterAttribute(CritChance);

        // Magic
        map[UnitStat.MagicAttack] = new CharacterAttribute(MagicAttack);
        map[UnitStat.Mana] = new CharacterAttribute(Mana);

        // Defense
        map[UnitStat.Health] = new CharacterAttribute(Health);
        map[UnitStat.Defense] = new CharacterAttribute(Defense);
        map[UnitStat.MagicDefense] = new CharacterAttribute(MagicDefense);
        map[UnitStat.Evasion] = new CharacterAttribute(Evasion);

        // Movement
        map[UnitStat.Initiative] = new CharacterAttribute(initative);

        return map;
    }
}


