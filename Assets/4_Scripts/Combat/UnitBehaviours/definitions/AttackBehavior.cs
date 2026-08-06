using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Behaviors/Attack")]
public class AttackBehavior : GridUnitBehaviourSO
{
    public override void Tick(UnitContext ctx)
    {
        if (!SetTarget(ctx)) return;

        if (ctx.IsInRange(ctx.Target.Grid.anchorCell))
        {
            PerformAttack(ctx);
            return;
        }
        else
        {
            ctx.Grid.WalkTowardsCell(ctx.Target.Grid.anchorCell);
        }
    }

    public bool CanAttack(UnitContext ctx)
    {
        return Time.time >= ctx.Stats.lastAttackTime + 1 / ctx.Stats.stats[UnitStat.AttackSpeed].Value;
    }

    public bool SetTarget(UnitContext ctx)
    {
        if (ctx.IsEngaged()) return true;
        Team otherTeam = ctx.Stats.team.GetOpposite();
        ctx.Target = MovementSystem.Instance.FindClosestUnit(ctx.Grid.anchorCell, otherTeam);
        return ctx.Target != null;
    }
    
    public void PerformAttack(UnitContext ctx)
    {
        if (!CanAttack(ctx)) return;
        ctx.Grid.ResetMovement();
        ctx.Stats.lastAttackTime = Time.time;
        ctx.Visuals.OnAttack(ctx, ResolveAttack);
    }

    public void ResolveAttack(UnitContext ctx)
    {
        if (ctx.Target == null || ctx.Target.Health.IsDead)
        {
            ctx.Target = null;
            ctx.Visuals.ResetVisuals();
            return;
        }
        int amount = ctx.Stats.stats[UnitStat.Attack].ToInt();
        Damage.Create(ctx, ctx.Target, DamageType.Physical, amount);
    }
}
