using UnityUtils;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using DG.Tweening;
using System.Collections;



public class FloatingTextManager : Singleton<FloatingTextManager>
{
    [SerializeField] private List<ObjectPoolSettings> numberSettings;

    private ObjectPool objectPool;
    private Camera _cam;
    public Camera cam
    {
        get {
            if (_cam == null) _cam = Camera.main;
            return _cam;
        }
    }

    protected override void Awake()
    {
        objectPool = new(numberSettings, this);
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        Damage.OnDamageResolved += OnDamage;
    }

    private void OnDisable()
    {
        Damage.OnDamageResolved -= OnDamage;
    }

    private void OnDamage(Damage dmg)
    {
        ObjectPoolSettings settings = numberSettings.First();
        foreach (Unit t in dmg.targets)
        {
            Vector3 position = t.transform.position + Vector3.up;
            DamageText obj = objectPool.Spawn(settings, position).gameObject.GetComponent<DamageText>();
            obj.SetDamage(dmg, objectPool);
        }
    }
}


