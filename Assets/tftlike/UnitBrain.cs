using UnityEngine;

[DefaultExecutionOrder(+10)]
public class UnitBrain : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private GridUnitBehaviourSO behavior;

    [field: SerializeReference] public UnitContext Context { get; private set; }

    private void Awake()
    {
        Context = new(this);
        Context.Health.OnDeath += HandleDeath;
    }

    private void OnEnable()
    {
        MovementSystem.Instance.RegisterUnit(this, Context.Stats.team);
    }

    private void OnDisable()
    {
        MovementSystem.Instance.UnregisterUnit(this, Context.Stats.team);
    }


    private void Update()
    {
        if (Context.Health.IsDead) return;


        //Context.Status?.Tick();
        behavior.Tick(Context);
    }

    private void HandleDeath()
    {
        Context.Visuals?.ResetVisuals();
        Context.Grid.ClearDesiredStep();
        GridSystem.Instance.RemoveUnit(Context.Grid);
        enabled = false;
        Destroy(this.gameObject);
    }
}
