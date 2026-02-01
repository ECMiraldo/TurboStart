using UnityEngine;

[CreateAssetMenu(menuName = "Units/Behaviors/Attack")]
public class AttackBehavior : GridUnitBehaviourSO
{
    public override void Tick(UnitContext ctx)
    {
        if (!SetTarget(ctx)) return;

        if (ctx.IsInRange(ctx.Target.Grid.anchorCell))
        {
            ctx.Grid.ClearDesiredStep();
            PerformAttack(ctx);
            return;
        }
        else
        {
            if (MovementSystem.Instance.gridPathFinder.TryGetNextStep(
                ctx.Grid,
                ctx.Grid.anchorCell,
                ctx.Target.Grid.anchorCell,
                out var step))
            {
                ctx.Grid.SetDesiredStep(step);
            }
            ctx.Target = null;
        }

       
    }

    public bool CanAttack(UnitContext ctx)
    {
        return Time.time >= ctx.Stats.lastAttackTime + 1 / ctx.Stats.attackSpeed;
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
        ctx.Stats.lastAttackTime = Time.time;


        ctx.Target.Health.TakeDamage(ctx.Stats.attackDamage);
        DamageNumberManager.ShowNumber(ctx.Target.Grid.transform.position, ctx.Stats.attackDamage.ToString());

        ctx.Visuals.OnAttack(ctx);
        if (ctx.Target.Health.IsDead || ctx.Target == null)
        {
            ctx.Target = null;
            ctx.Visuals.ResetVisuals();
        }
    }
}
