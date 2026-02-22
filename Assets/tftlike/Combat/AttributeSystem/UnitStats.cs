using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using UnityEngine;

public enum UnitStat
{
    Attack,
    MagicAttack,
    Accuracy,
    CritChance,
    Health,
    Mana,
    Defense,
    MagicDefense,
    Evasion,
    Initiative,


}



[Serializable]
public class UnitStats
{
    [field: SerializeField] public SerializedDictionary<UnitStat, CharacterAttribute> stats { get; private set; }

    public UnitStats(IUnitData data)
    { 
        stats = data.GetStatsMap();
    }

    [JsonConstructor]
    public UnitStats(SerializedDictionary<UnitStat, CharacterAttribute> stats)
    {
        this.stats = stats;
    }
}


