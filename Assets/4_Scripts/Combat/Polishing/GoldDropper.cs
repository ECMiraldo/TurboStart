using System;
using UnityEngine;

public class ExperienceDropper : MonoBehaviour
{
    [SerializeField] private ObjectPoolSettings crystalPoolSettings;
    private ObjectPool experienceDropPool;

    private void Awake()
    {
        experienceDropPool = new ObjectPool(crystalPoolSettings, this);
    }

    private void OnEnable()
    {
        HealthComponent.OnUnitDied += HandleUnitDeath;
    }

    private void OnDisable()
    {
        HealthComponent.OnUnitDied -= HandleUnitDeath;
    }

    private void HandleUnitDeath(StatsComponent stats)
    {
        if (stats is HeroStatsComponent)
            return;
        
        if (stats is EnemyStatsComponent enemyStats)
        {
            IPooledObject drop = experienceDropPool.Spawn(crystalPoolSettings, stats.transform.position);
            ExperienceCrystal experienceCrystal = drop.gameObject.GetComponent<ExperienceCrystal>();
            experienceCrystal.SetDestroyCb(() => Despawn(drop));
        }
        
    }

    private void Despawn(IPooledObject obj)
    {
        experienceDropPool.Despawn(obj);
    }
}