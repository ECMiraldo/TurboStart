using UnityEngine;
using System.Collections;
using System;

public class ProjectileAnimator : AttackAnimator
{
    [SerializeField] private ObjectPoolSettings projectileSettings;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float projectileSpeed = 10;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private bool faceTarget = true;

    private ObjectPool objectPool;
    private WaitForSeconds spawnYield;

    protected override void Awake()
    {
        objectPool = new ObjectPool(projectileSettings, this);
        spawnYield = new WaitForSeconds(spawnDelay);
    }

    private void OnDisable()
    {
        objectPool.DespawnAll();
    }


    public override IEnumerator OnAttack(UnitContext ctx, Action<UnitContext> callback)
    {
        animator.SetTrigger(attackHash);
        yield return spawnYield;

        
        IPooledObject obj = objectPool.Spawn(projectileSettings, spawnPos.position);
        obj.transform.SetParent(null); // keep world position
        obj.transform.position = spawnPos.position;

        float elapsed = 0f;
        if (ctx.Target == null || ctx.Target.Health.IsDead)
        {
            callback?.Invoke(ctx);
            objectPool.Despawn(obj);
        }

        Vector3 targetPos = ctx.Target.Grid.transform.position;

        while (Vector3.Distance(obj.transform.position,targetPos) > 0.1f)
        {
            if (ctx.Target == null || ctx.Target.Health.IsDead)
                break;

            targetPos = ctx.Target.Grid.transform.position;
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                targetPos,
                projectileSpeed * Time.deltaTime
            );

            if (faceTarget)
            {
                obj.transform.right = (targetPos - obj.transform.position).normalized;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke(ctx);
        objectPool.Despawn(obj);
    }

}
