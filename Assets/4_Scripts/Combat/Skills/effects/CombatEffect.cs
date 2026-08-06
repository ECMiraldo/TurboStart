using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class CombatEffect
{
    [SerializeField] public float animationYield;
    public abstract IEnumerator Execute(UnitContext caster);
}
