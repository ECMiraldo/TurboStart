using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;


public enum Team : byte { Player, Enemy, Neutral}

public abstract class StatsComponent : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public Team team { get; protected set; }
    [field: SerializeField] public SerializedDictionary<UnitStat, Attribute> stats { get; protected set; }

    [field: Header("State")]
    [field: ReadOnly] public float lastAttackTime = 0;

    protected UnitContext ctx { get; private set; }
    public virtual void Init(UnitContext ctx)
    {
        this.ctx = ctx;
    }

}
