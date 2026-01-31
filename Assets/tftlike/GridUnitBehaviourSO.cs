using UnityEngine;


public abstract class GridUnitBehaviourSO : ScriptableObject
{
    public abstract void Tick(UnitContext ctx);
}
