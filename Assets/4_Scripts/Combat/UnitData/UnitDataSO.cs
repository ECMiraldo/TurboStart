using AYellowpaper.SerializedCollections;
using UnityEngine;

public abstract class UnitDataSO : IDScriptableObject
{
    [Header("General")]
    public GameObject prefab;
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; } = new Vector2Int[] { Vector2Int.zero };


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

    public SerializedDictionary<UnitStat, CharacterAttribute> GetStats()
    {
        return new()
        {
            { UnitStat.Health, new CharacterAttribute(health) },
            { UnitStat.Mana, new CharacterAttribute(mana) },
            { UnitStat.Attack, new CharacterAttribute(attack) },
            { UnitStat.MagicAttack, new CharacterAttribute(magicAttack) },
            { UnitStat.AttackSpeed, new CharacterAttribute(attackSpeed) },
            { UnitStat.AttackRange, new CharacterAttribute(attackRange) },
            { UnitStat.Accuracy, new CharacterAttribute(accuracy) },
            { UnitStat.CritChance, new CharacterAttribute(critChance) },
            { UnitStat.Defense, new CharacterAttribute(defense) },
            { UnitStat.MagicDefense, new CharacterAttribute(magicDefense) },
            { UnitStat.Evasion, new CharacterAttribute(evasion) },
        };
    }


}
