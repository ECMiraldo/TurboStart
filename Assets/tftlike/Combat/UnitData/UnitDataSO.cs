using AYellowpaper.SerializedCollections;
using UnityEngine;

public abstract class UnitDataSO : IDScriptableObject
{
    [Header("General")]
    public GameObject prefab;
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; }


    [Header("Stats")]
    public int baseHP;
    public int baseDamage;
    public float attackSpeed;
    public int attackRange;


    public SerializedDictionary<UnitStat, CharacterAttribute> GetStats()
    {
        return new();

    }


}
