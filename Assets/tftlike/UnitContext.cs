using System;
using UnityEngine;

[Serializable]
public sealed class UnitContext
{
    public readonly GridUnit Grid;


    public readonly GridUnitStats Stats;
    //public StatusController Status;


    public readonly GridHealthComponent Health;
    public readonly GridAttackComponent Attack;


    public readonly CombatVisuals Visuals;


    // Runtime state (owned by the unit, not behaviors)
    public UnitContext Target = null;

    public UnitContext(UnitBrain brain)
    {
        Grid = brain.GetComponent<GridUnit>();
        Stats = brain.GetComponent<GridUnitStats>();
        // Status = GetComponent<StatusController>(),
        Health = brain.GetComponent<GridHealthComponent>();
        Attack = brain.GetComponent<GridAttackComponent>();
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
