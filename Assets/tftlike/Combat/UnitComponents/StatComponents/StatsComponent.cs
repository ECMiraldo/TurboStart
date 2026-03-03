using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;


public enum Team : byte { Player, Enemy, Neutral}
public enum UnitStat : byte
{
    Attack,
    MagicAttack,
    AttackSpeed,
    AttackRange,
    Accuracy,
    CritChance,
    Health,
    Mana,
    Defense,
    MagicDefense,
    Evasion,
}

public abstract class StatsComponent : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public Team team { get; protected set; }
    [field: SerializeField] public SerializedDictionary<UnitStat, CharacterAttribute> stats { get; protected set; }

    [field: Header("State")]
    [field: ReadOnly] public float lastAttackTime = 0;
    public abstract void Init(UnitContext ctx);

}
