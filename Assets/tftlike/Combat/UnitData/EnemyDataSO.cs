using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyDataSO : UnitDataSO
{
    [Header("Spawn Cost")]
    public int powerCost = 1; // lower = fodder, higher = elite

    [Header("Positioning")]
    public Positioning positioning;

}
