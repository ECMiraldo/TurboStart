using UnityEngine;
using System;

[DefaultExecutionOrder(+10)]
public class UnitBrain : MonoBehaviour
{
    public static event Action<UnitBrain, Team> onUnitSpawned;
    public static event Action<UnitBrain, Team> onUnitDespawned;


    [Header("Behavior")]
    [SerializeField] private GridUnitBehaviourSO behavior;

    [field: SerializeReference] public UnitContext Context { get; private set; }
    [field: SerializeReference] public bool isEnabled { get; private set; }
    public void SetEnabled(bool enabled) => isEnabled = enabled;
    private void Awake()
    {
        Context = new(this);
        Context.Health.OnDeath += HandleDeath;
    }

    private void OnEnable()
    {
        onUnitSpawned?.Invoke(this, Context.Stats.team);
    }

    private void OnDisable()
    {
        onUnitDespawned?.Invoke(this, Context.Stats.team);
    }

   

    private void Update()
    {
        if (!isEnabled) return;

        if (Context.Health.IsDead) return;


        //Context.Status?.Tick();
        behavior.Tick(Context);
    }

    private void HandleDeath()
    {
        Context.Visuals?.ResetVisuals();
        Context.Grid.ResetMovement();
        GridSystem.Instance.RemoveUnit(Context.Grid);
        enabled = false;
        Destroy(this.gameObject);
    }
}
