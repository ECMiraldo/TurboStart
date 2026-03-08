using UnityEngine;
using Persistence;

[RequireComponent(typeof(EnemyStatsComponent), typeof(HealthComponent))]
public class EnemyRewardComponent : MonoBehaviour
{

    private HealthComponent healthComponent;
    private EnemyStatsComponent enemyStats;
    private void Start()
    {
        enemyStats = GetComponent<EnemyStatsComponent>();
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnDeath += GiveRewards;
    }

    private void OnDestroy()
    {
        healthComponent.OnDeath -= GiveRewards;
    }

    private void GiveRewards()
    {
        ResultUI.instance.AddGoldToLoot(enemyStats.enemyDataSO.goldReward);
        foreach (var loot in enemyStats.enemyDataSO.lootTable.GetDrops())
        {
            ResultUI.instance.AddItemToLoot(loot.ToItem());
        }
    }




}
