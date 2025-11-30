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
    private Camera cam;

    protected override void Awake()
    {
        objectPool = new(numberSettings, this);
        cam = Camera.main;
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
            Vector3 screenPos = cam.WorldToScreenPoint(t.transform.position);
            DamageText obj = objectPool.Spawn(settings, screenPos).gameObject.GetComponent<DamageText>();
            obj.SetDamage(dmg, objectPool);
        }
    }
}


