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

        GameObject projectileGO = GameObject.Instantiate(projectile.projectilePrefab, caster.Grid.transform.position, Quaternion.identity);

        float elapsed = 0f;

        
        Vector3 targetPos = caster.Target.Grid.transform.position;

        while (Vector3.Distance(projectileGO.transform.position,targetPos) > 0.1f)
        {
            if (caster.Target == null || caster.Target.Health.IsDead)
                break;

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

        foreach (CombatEffect effect in projectile.onDamageEffects)
        {
            yield return effect.Execute(caster);
        }

        GameObject.Destroy(projectileGO);
    }
}
