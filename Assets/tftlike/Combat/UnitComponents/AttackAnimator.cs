using System.Collections;
using UnityEngine;
using System;

public class AttackAnimator : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected float damageDelay;


    protected int attackHash = Animator.StringToHash("Attack");
    protected WaitForSeconds damageYield;


    protected virtual void Awake()
    {
        damageYield = new WaitForSeconds(damageDelay);   
    }

    public virtual IEnumerator OnAttack(UnitContext ctx, Action<UnitContext> callback)
    {
        animator.SetTrigger(attackHash);
        yield return damageYield;
        callback?.Invoke(ctx);

        
    }
}
