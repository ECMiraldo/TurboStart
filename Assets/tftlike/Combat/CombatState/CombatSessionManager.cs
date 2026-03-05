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

public class CombatSessionManager : MonoBehaviour
{
    public static event Action<CombatState> onStateChanged;
    public static event Action<bool> onAutoplayToggled;
    public static event Action onRoundWon;
    public static event Action onStageWon;
    public static event Action onStageLost;
    public static CombatSessionManager Instance { get; private set; }

    [field: SerializeField] public CombatSpawner spawner { get; private set; }
    [field: SerializeField] public StageDefinitionSO currentStage { get; private set; }
    [field: SerializeField, ReadOnly] public int currentRoundIndex { get; private set; }
    [field: SerializeField, ReadOnly] public RoundDefinitionSO currentRound { get; private set; }

    [field: SerializeField] public bool isAutoplay { get; private set; }
    
    [SerializeField] private float preRoundDelay = 1.5f;
    [SerializeField] private float postRoundDelay = 2f;
    [SerializeField] private float spawningDelay = 2f;

    public readonly Dictionary<Team, List<UnitBrain>> unitsByTeam = new();
    public IEnumerable<UnitBrain> allUnits => unitsByTeam[Team.Player].Concat(unitsByTeam[Team.Enemy]).Concat(unitsByTeam[Team.Neutral]);
    [field: SerializeField, ReadOnly] public CombatState State { get; private set; }

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
        onAutoplayToggled?.Invoke(this);
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

        SetState(CombatState.StageSetup); 
    }


    public void BeginNextRound()
    {
        if (currentRoundIndex >= currentStage.rounds.Count)
        {
            SetState(CombatState.StageComplete);
            if (isAutoplay) ResetStage();
            return;
        }
        
        currentRound = currentStage.rounds[currentRoundIndex];
        currentRoundIndex++;

        StartCoroutine(RoundFlow());
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
        if (unitsByTeam[Team.Enemy].Count == 0)
        {
            onRoundWon?.Invoke();
        }
        //ROUND LOST
        else
        {
            if (isAutoplay)
            {
                ResetStage();
                yield return new WaitForSeconds(postRoundDelay / 2);
                BeginNextRound();
                yield break;
            }
            else
            {
                yield break;
            }
        }
             
        yield return new WaitForSeconds(postRoundDelay / 2);
        ResetHeroUnitsPosition();
        yield return new WaitForSeconds(postRoundDelay);

        // NEXT ROUND
        BeginNextRound();
    }

    private void HandleDefeat()
    {
        onStageLost?.Invoke();
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
        foreach (UnitBrain ctx in unitsByTeam[Team.Enemy].Concat(unitsByTeam[Team.Neutral]).Concat(unitsByTeam[Team.Player]))
        {
            Destroy(ctx.gameObject);
        }
        spawner.ReplaceHeroes();
        //check the heroes that are alive and reset them
        StartStage(currentStage);
       
    }
}
