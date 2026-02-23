using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;


public enum Team : byte { Player, Enemy, Neutral}
public enum UnitStat : byte
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
}

public abstract class StatsComponent : MonoBehaviour
{
    [field: SerializeField] public Team team { get; protected set; }
    [field: SerializeField] public SerializedDictionary<UnitStat, CharacterAttribute> stats { get; protected set; }

    [field: Header("State")]
    [field: ReadOnly] public float lastAttackTime = 0;


    public int maxHealth = 100;
    public int currentHealth = 100;
    public int attackDamage = 10;
    public float attackSpeed = 2.0f;
    public int attackRange = 1;

    public abstract void Init(UnitContext ctx);

}
