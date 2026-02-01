using UnityEngine;

public enum CombatState
{
    Idle,
    StageSetup,      // place heroes (later)
    PreRound,        // UI polish, countdown
    Spawning,
    Combat,
    PostRound,       // rewards UI
    StageComplete
}

public class CombatSession : MonoBehaviour
{
    public static CombatSession Instance { get; private set; }

    public CombatState State { get; private set; }

    [SerializeField] private StageController stageController;

    private void Awake()
    {
        Instance = this;
    }

    public void SetState(CombatState newState)
    {
        State = newState;
        Debug.Log($"GameState → {newState}");
    }
}
