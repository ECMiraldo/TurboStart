using System.Collections;
using UnityEngine;


public abstract class Unit : MonoBehaviour
{
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public UnitMoveResolver moveResolver { get; private set; }
    public BattleManager battleManager { get; private set; }
    public abstract UnitStats stats { get; }
    public abstract UnitVitals unitVitals { get; }

    protected virtual void Start()
    {
        unitVitals.FullRegen();
    }


    public void SetBattleManager(BattleManager manager) => this.battleManager = manager; 
    public void TakeDamage(Damage damage)
    {
        unitVitals.IncrementHealth(-damage.amount);
        if (unitVitals.currentHealth > 0)
        {
            animator.SetTrigger(Constants.animationHashes[AnimationNames.Hurt]);
        }
        else
        {
            animator.SetTrigger(Constants.animationHashes[AnimationNames.Die]);
        }
    }

    public IEnumerator DoMove() 
    {
        yield return moveResolver.DoMove();  
    }
}


