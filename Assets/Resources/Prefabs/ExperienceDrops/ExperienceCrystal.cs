using System;
using UnityEngine;

public class ExperienceCrystal : MonoBehaviour, IPooledObject
{
    [field: SerializeField] public ObjectPoolSettings poolSettings {get; private set;}
    private Action despawnCallback;
    public void SetDestroyCb(Action despawn) 
    {
        this.despawnCallback = despawn;
    }

    private void OnMouseEnter()
    {
        CombatSessionManager.Instance.IncreaseStageExperience();
        despawnCallback?.Invoke();
    }
  
}
