using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleMoveController : MonoBehaviour
{
    [field: SerializeField] public Unit unit { get; private set; }
    [field: SerializeField] public UnitBattleMoveSO moveData { get; private set; }
    protected WaitForSeconds yieldTime;



    protected virtual void Awake()
    {
        yieldTime = new WaitForSeconds(moveData.moveTime);
        unit = transform.parent.GetComponentInParent<Unit>();
    }

    public virtual IEnumerator DoMove()
    {
        unit.animator.SetTrigger(Constants.animationHashes[moveData.animationName]);
        yield return yieldTime;
        yield return ResolveMove(moveData.GetTargets(unit, unit is HeroBattleController));
    }

    public abstract IEnumerator ResolveMove(IEnumerable<Unit> targets);

}

 
