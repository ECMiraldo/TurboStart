using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ProjectileEffect : CombatEffect
{
    [SerializeField] private ProjectileSO projectile;
    
    public override IEnumerator Execute(UnitContext caster)
    {
        yield return new WaitForSeconds(animationYield);

        if (caster.Target == null || caster.Target.Grid == null || caster.Target.Health.IsDead)
        {
            yield break;
        }

        //get casted skill
        SkillSO castedSkill = caster.Stats.unitData.skill;
        int totalFocus = caster.Focus.CurrentFocus;
        caster.Focus.ConsumeAll();
        GameObject projectileGO = GameObject.Instantiate(projectile.projectilePrefab, caster.Grid.transform.position, Quaternion.identity, null);

        float elapsed = 0f;

        
        Vector3 targetPos = caster.Target.Grid.transform.position;

        while (Vector3.Distance(projectileGO.transform.position,targetPos) > 0.1f)
        {
            if (caster.Target == null || caster.Target.Health.IsDead)
            {
                GameObject.Destroy(projectileGO);
                yield break;
            }

            targetPos = caster.Target.Grid.transform.position;
            projectileGO.transform.position = Vector3.MoveTowards(
                projectileGO.transform.position,
                targetPos,
                projectile.speed * Time.deltaTime
            );

            projectileGO.transform.right = (targetPos - projectileGO.transform.position).normalized;

            elapsed += Time.deltaTime;
            yield return null;
        }

        int effectValue = castedSkill.GetEffectValue(caster.Stats, totalFocus );
        Damage.Create(caster, caster.Target, castedSkill.damageType, effectValue);

        foreach (CombatEffect effect in projectile.onDamageEffects)
        {
            yield return effect.Execute(caster);
        }

        GameObject.Destroy(projectileGO);
    }
}
