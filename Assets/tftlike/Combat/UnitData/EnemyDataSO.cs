using UnityEngine;





[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyDataSO : UnitDataSO
{
    [Header("Spawning")]
    public int powerCost = 1; // lower = fodder, higher = elite
    public Positioning positioning;

    [Header("Rewards")]
    public int goldReward;
    public LootTable lootTable;


}
