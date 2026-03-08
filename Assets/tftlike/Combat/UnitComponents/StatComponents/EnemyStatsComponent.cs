using UnityEngine;

public class EnemyStatsComponent : StatsComponent
{
    [field: SerializeField] public EnemyDataSO enemyDataSO { get; private set; }
    
    public override void Init(UnitContext ctx)
    {
        team = Team.Enemy;
        stats = enemyDataSO.GetStats();
    }

}
