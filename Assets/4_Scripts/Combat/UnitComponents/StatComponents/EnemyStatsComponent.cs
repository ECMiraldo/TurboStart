using UnityEngine;

public class EnemyStatsComponent : StatsComponent
{
    [field: SerializeField] public EnemyDataSO enemyDataSO { get; private set; }
    public override UnitDataSO unitData => enemyDataSO;

    public override void Init(UnitContext ctx)
    {
        base.Init(ctx);
        team = Team.Enemy;
        float roundPowerMultiplier = (CombatSessionManager.Instance.currentRoundIndex + 1) *
                                    CombatSessionManager.Instance.currentStage.enemyRoundMultiplier;
        stats = enemyDataSO.GetStats(roundPowerMultiplier);
    }

}
