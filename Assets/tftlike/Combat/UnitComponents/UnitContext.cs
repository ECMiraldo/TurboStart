using System;
using UnityEngine;

[Serializable]
public sealed class UnitContext
{
    public readonly GridComponent Grid;


    public readonly UnitStats Stats;
    //public StatusController Status;


    public readonly HealthComponent Health;
   // public readonly GridAttackComponent Attack;


    public readonly CombatVisuals Visuals;


    // Runtime state (owned by the unit, not behaviors)
    public UnitContext Target = null;

    public UnitContext(UnitBrain brain)
    {
        Grid = brain.GetComponent<GridComponent>();
        Stats = brain.GetComponent<UnitStats>();
        // Status = GetComponent<StatusController>(),
        Health = brain.GetComponent<HealthComponent>();
       // Attack = brain.GetComponent<GridAttackComponent>();
        Visuals = brain.GetComponentInChildren<CombatVisuals>();
    }



    public bool IsEngaged()
    {
        return Target != null
            && !Target.Health.IsDead
            &&  IsInRange(Target.Grid.anchorCell);
    }

    public bool IsInRange(Vector2Int to, float range = 0)
    {
        if (range == 0) range = Stats.attackRange;

        int dx = Mathf.Abs(Grid.anchorCell.x - to.x);
        int dy = Mathf.Abs(Grid.anchorCell.y - to.y);

        return Mathf.Max(dx, dy) <= range;
    }
}
