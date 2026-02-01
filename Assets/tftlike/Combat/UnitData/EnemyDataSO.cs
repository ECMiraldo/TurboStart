using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public string id;

    [Header("Stats")]
    public int baseHP;
    public int baseDamage;
    public float attackSpeed;
    public int attackRange;

    [Header("Spawn Cost")]
    public int powerCost = 1; // lower = fodder, higher = elite

    [Header("Positioning")]
    public Positioning positioning;
    public Vector2Int[] footprintOffsets;

    [Header("Prefab")]
    public GameObject prefab;
}
