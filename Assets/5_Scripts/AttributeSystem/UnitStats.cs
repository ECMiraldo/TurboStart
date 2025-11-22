



using AYellowpaper.SerializedCollections;
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



public class UnitStats
{
    [field: SerializeField] public SerializedDictionary<UnitStat, CharacterAttribute> stats { get; private set; }

    public UnitStats(IUnitData data)
    { 
        stats = data.GetStatsMap();
    }
}


