using System.Collections;
using System.Collections.Generic;

public class BasicAttackMove : BattleMoveController
{
    public override IEnumerator ResolveMove(IEnumerable<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            target.TakeDamage(new Damage { amount = 1 });
        }
        yield return null;
    }
}

 
