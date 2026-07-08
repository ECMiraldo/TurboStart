using UnityEngine;





[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyDataSO : UnitDataSO
{
    [Header("Spawning")]
    public int powerCost = 1; // lower = fodder, higher = elite
    public Positioning positioning;
    public LootTable lootTable;


}
