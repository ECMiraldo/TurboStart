using UnityEngine;

[CreateAssetMenu(menuName = "Units/Behaviors/Attack")]
public class AttackBehavior : GridUnitBehaviourSO
{
    public override void Tick(UnitContext ctx)
    {
        // if (ctx.Status.IsStunned) return;
        if (ctx.IsEngaged())
        {
            ctx.Grid.ClearPath();
            PerformAttack(ctx);
            return;
        }

        Team otherTeam = ctx.Stats.team.GetOpposite();
        ctx.Target = MovementSystem.Instance.FindClosestUnit(otherTeam);
        if (ctx.Target == null) return;


        if (!ctx.Attack.IsInRange(
            ctx.Grid.anchorCell,
            ctx.Target.Grid.anchorCell))
        {
            if (MovementSystem.Instance.gridPathFinder.TryFindPathToRange(
                ctx.Grid, ctx.Grid.anchorCell, ctx.Target.Grid.anchorCell, ctx.Stats.attackRange, out var path)
                )
            {
                ctx.Grid.SetPath(path);
            }
        }
        else
        {
            ctx.Grid.ClearPath();
            PerformAttack(ctx);
        }
    }

    public bool CanAttack(UnitContext ctx)
    {
        return Time.time >= ctx.Stats.lastAttackTime + 1 / ctx.Stats.attackSpeed;
    }
    
    public void PerformAttack(UnitContext ctx)
    {
        if (!CanAttack(ctx)) return;
        ctx.Stats.lastAttackTime = Time.time;


        ctx.Target.Health.TakeDamage(ctx.Stats.attackDamage);
        DamageNumberManager.ShowNumber(ctx.Target.Grid.transform.position, ctx.Stats.attackDamage.ToString());

        ctx.Visuals.OnAttack(ctx);
        if (ctx.Target.Health.IsDead)
        {
            ctx.Target = null;
            ctx.Visuals.ResetVisuals();
        }
    }
}
