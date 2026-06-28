using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ItemEffect
{
    [SerializeReference, SubclassSelector] public ItemEffectCondition condition = null;
    [SerializeReference, SubclassSelector] public ItemEffectConsequence consequence = null;

    public void OnEnable(UnitContext context) => condition.OnEnable(context);

    public void CheckEffect(UnitContext context)
    {
        if (condition.CheckCondition(context))
        {
            context.Stats.StartCoroutine(consequence.Execute(context));
        }
    }
}

[Serializable]

public abstract class ItemEffectCondition
{
    public abstract void OnEnable(UnitContext context);

    public abstract void OnDisable(UnitContext context);
    public abstract bool CheckCondition(UnitContext context);
}

[Serializable]

public abstract class ItemEffectConsequence
{
    public abstract IEnumerator Execute(UnitContext context);
}

[Serializable]
public class DamageConsequence : ItemEffectConsequence
{
    [SerializeField] private int damage;
    public override IEnumerator Execute(UnitContext context)
    {
        context.Target.Health.TakeDamage(damage);
        yield return null;
    }
}

[Serializable]
public class EveryXHits : ItemEffectCondition
{
    [SerializeField] private int nHits = 3;
    private int currentHits = 0;
    public override void OnEnable(UnitContext context) => context.Visuals.onAttack += CountHit;
    public override void OnDisable(UnitContext context) => context.Visuals.onAttack -= CountHit;
    public override bool CheckCondition(UnitContext context) => currentHits % nHits == 0;
    private void CountHit(UnitContext ctx) => currentHits++;


}