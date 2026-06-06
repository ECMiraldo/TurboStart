using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
[DefaultExecutionOrder(-1)]
public class CombatSessionManager : MonoBehaviour
{
    public static CombatSessionManager Instance { get; private set; }


    public static event Action<CombatState> onStateChanged;
    public static event Action<bool> onAutoplayToggled;
    public static event Action onRoundWon; 

    [Header("Refs")]
    [field: SerializeField] public CombatSpawner spawner { get; private set; }
    [field: SerializeField] public ResultUI resultUI { get; private set; }
    [field: SerializeField] public StageDefinitionSO currentStage { get; private set; }
    [field: SerializeField] public GameObject UI { get; private set; }

    [Header("Configs")]
    [field: SerializeField] public bool isAutoplay { get; private set; } = false;
    [SerializeField] private float preRoundDelay = 1.5f;
    [SerializeField] private float postRoundDelay = 2f;
    [SerializeField] private float spawningDelay = 2f;
    [SerializeField] public float resultScreenTime = 3.0f;


    [Header("State")]
    [field: SerializeField, ReadOnly] public int currentRoundIndex { get; private set; }
    [field: SerializeField, ReadOnly] public RoundDefinitionSO currentRound { get; private set; }
    [field: SerializeField, ReadOnly] public CombatState State { get; private set; }

    public readonly Dictionary<Team, List<UnitBrain>> unitsByTeam = new();


    public IEnumerable<UnitBrain> allUnits => unitsByTeam[Team.Player].Concat(unitsByTeam[Team.Enemy]).Concat(unitsByTeam[Team.Neutral]);

    private Coroutine currentRoutine;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
        unitsByTeam[Team.Enemy] = new();
        unitsByTeam[Team.Player] = new();
        unitsByTeam[Team.Neutral] = new();
    }

    public void SetAutoplay(bool autoplay)
    {
        this.isAutoplay = autoplay;
        onAutoplayToggled?.Invoke(autoplay);
    }

    private void OnEnable()
    {
        UnitBrain.onUnitSpawned += RegisterUnit;
        UnitBrain.onUnitDespawned += UnregisterUnit;
    }

    private void OnDisable()
    {
        UnitBrain.onUnitSpawned -= RegisterUnit;
        UnitBrain.onUnitDespawned -= UnregisterUnit;
    }

    public void RegisterUnit(UnitBrain unit, Team team)
    {
        if (!unitsByTeam[team].Contains(unit))
        {
            unitsByTeam[team].Add(unit);
        }

    }

    public void UnregisterUnit(UnitBrain unit, Team team)
    {
        unitsByTeam[team].Remove(unit);
        if (GridSystem.Instance != null)
            GridSystem.Instance.RemoveUnit(unit.Context.Grid);
    }

    public void SetState(CombatState newState)
    {
        State = newState;
        onStateChanged?.Invoke(newState);
        Debug.Log($"GameState → {newState}");
    }

    public void StartStage(StageDefinitionSO stage)
    {
        currentStage = stage;
        currentRoundIndex = 0;
        UI.SetActive(true);
        SetState(CombatState.StageSetup); 
    }


    public void BeginNextRound()
    {
        if (currentRoundIndex >= currentStage.rounds.Count)
        {
            SetState(CombatState.StageComplete);
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(HandleVictory());
            return;
        }
        
        currentRound = currentStage.rounds[currentRoundIndex];
        currentRoundIndex++;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RoundFlow());
    }


    private IEnumerator RoundFlow()
    {

        // PRE-ROUND
        SetState(CombatState.PreRound);
        yield return new WaitForSeconds(preRoundDelay);

        // SPAWN
        SetState(CombatState.Spawning);
        spawner.SpawnRound(currentRound);

        yield return new WaitForSeconds(spawningDelay); ; // one frame safety

        // COMBAT
        MovementSystem.Instance.SetActiveUnits(allUnits);
        SetState(CombatState.Combat);
        ToggleUnits(true);
        yield return new WaitUntil(IsRoundOver);


        // POST-ROUND
        SetState(CombatState.PostRound);
       
        ToggleUnits(false);

        //ROUND WON

        //CHECK ROUND LOST FIRST HERE
        if (unitsByTeam[Team.Player].Count == 0)
        {
            yield return HandleDefeat();
            yield break;
        }
        else
        {
            onRoundWon?.Invoke();
        }
             
        yield return new WaitForSeconds(postRoundDelay / 2);
        ResetHeroUnitsPosition();
        yield return new WaitForSeconds(postRoundDelay);

        // NEXT ROUND
        BeginNextRound();
    }

    public void PlayerClickedRestart()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        ResetStage();
        BeginNextRound();
    }

    public void PlayerClickedLeave()
    {
        DestroyUnits(unitsByTeam[Team.Enemy].Concat(unitsByTeam[Team.Neutral]));
    }

    private IEnumerator HandleDefeat()
    {
        resultUI.ShowDefeat();
        yield return RestartStage();
    }

    private IEnumerator HandleVictory()
    {
        //onStageWon?.Invoke(resultScreenTime);

        foreach (ItemSO item in currentStage.stageLootTable.GetDrops())
        {
            resultUI.AddItemToLoot(item.ToItem());
        }
        resultUI.ShowVictory();
        if (isAutoplay)
        {
            yield return RestartStage();
        }
       
    }

    private IEnumerator RestartStage()
    {
        yield return new WaitForSeconds(resultScreenTime);
        ResetStage();
        yield return null; //just wait a frame here
        BeginNextRound();
    }

    private bool IsRoundOver()
    {
        return unitsByTeam[Team.Enemy].Count == 0 || unitsByTeam[Team.Player].Count == 0;
    }

    private void ResetHeroUnitsPosition()
    {
        foreach (var unit in unitsByTeam[Team.Player])
        {
            unit.Context.Grid.JumpToCell(unit.Context.Grid.starterCell);
        }
    }
    private void ToggleUnits(bool val)
    {
        foreach (var unit in allUnits)
        {
            unit.SetEnabled(val);
        }
    }

    private void ResetStage()
    {
        DestroyUnits(unitsByTeam[Team.Enemy].Concat(unitsByTeam[Team.Neutral]).Concat(unitsByTeam[Team.Player]));
        spawner.ReplaceHeroes();
        //check the heroes that are alive and reset them
        StartStage(currentStage);
    }

    private void DestroyUnits(IEnumerable<UnitBrain> brains)
    {
        foreach (UnitBrain ctx in brains)
        {
            Destroy(ctx.gameObject);
        }
    }

}
