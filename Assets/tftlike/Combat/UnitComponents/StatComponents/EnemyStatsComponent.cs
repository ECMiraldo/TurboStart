using UnityEngine;

public class EnemyStatsComponent : StatsComponent
{
    [field: SerializeField] private EnemyDataSO enemyDataSO;
    
    public override void Init(UnitContext ctx)
    {
        team = Team.Enemy;
        stats = enemyDataSO.GetStats();
    }

}
