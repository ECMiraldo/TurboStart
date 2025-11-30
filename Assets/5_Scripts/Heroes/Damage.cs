using System.Collections.Generic;
using System;

public struct Damage
{
    public static event Action<Damage> OnDamageResolved;

    public int amount;
    public Unit attacker;
    public List<Unit> targets;

    public Damage(int amount, Unit attacker, List<Unit> targets, bool canCrit = true, bool canMiss = false)
    {
        this.amount = amount;
        this.attacker = attacker;
        this.targets = targets;

        OnDamageResolved?.Invoke(this);
    }


}


