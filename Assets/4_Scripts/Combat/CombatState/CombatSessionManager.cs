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
    public static event Action<int> onStageExperienceChanged;

    [Header("Refs")]
    [field: SerializeField] public CombatSpawner spawner { get; private set; }
    [field: SerializeField] public ResultUI resultUI { get; private set; }
    [field: SerializeField] public UIHeroBoard heroBoard { get; private set; }
    [field: SerializeField] public StageDefinitionSO currentStage { get; private set; }
    [field: SerializeField] public GameObject UI { get; private set; }

    [Header("Configs")]
    [field: SerializeField] public bool isAutoplay { get; private set; } = false;
    [SerializeField] private float preRoundDelay = 1.5f;
    [SerializeField] private float postRoundDelay = 2f;
    [SerializeField] private float spawningDelay = 2f;
    [SerializeField] public float resultScreenTime = 3.0f;


    [Header("State")]
    [field: SerializeField, ReadOnly] public int currentRoundIndex { get; private set; } = 0;
    [field: SerializeField, ReadOnly] public int stageExperience {get; private set;} = 0;
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
        DestroyUnits(allUnits);
    }
    public HeroStatsComponent GetHeroStatsByData(HeroData data)
    {
        foreach (var unit in unitsByTeam[Team.Player])
        {
            if (unit.Context.Stats is HeroStatsComponent heroStats && heroStats.heroData == data)
            {
                return heroStats;
            }
         
        }
        return null;
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
        gameObject.SetActive(true);
        currentStage = stage;
        currentRoundIndex = 0;
        SetState(CombatState.StageSetup);
        heroBoard.gameObject.SetActive(true);
    }


    public void BeginNextRound()
    {
        if (currentRoundIndex >= currentStage.nRounds)
        {
            SetState(CombatState.StageComplete);
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(HandleVictory());
            return;
        }

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(RoundFlow());
    }


    private IEnumerator RoundFlow()
    {
        while (true)
        {
            // PRE-ROUND
            SetState(CombatState.PreRound);

            yield return new WaitForSeconds(preRoundDelay);

            // SPAWN
            SetState(CombatState.Spawning);
            spawner.SpawnRound(currentStage, currentRoundIndex);

            yield return new WaitForSeconds(spawningDelay); ; // one frame safety

            // COMBAT
            MovementSystem.Instance.SetActiveUnits(allUnits);
            SetState(CombatState.Combat);
            ToggleUnits(true);

            yield return new WaitUntil(IsRoundOver);

            // POST-ROUND
            SetState(CombatState.PostRound);
            ToggleUnits(false);

            //CHECK ROUND LOST FIRST HERE
            if (unitsByTeam[Team.Player].Count == 0)
            {
                yield return HandleDefeat();
                yield break;
            }
            else
            {
                //ROUND WON
                onRoundWon?.Invoke();
            }
                
            yield return new WaitForSeconds(postRoundDelay / 2);

            DestroyUnits(allUnits);
            spawner.ReplaceHeroes();

            yield return new WaitForSeconds(postRoundDelay);

            currentRoundIndex++;

        }
        
    }

    public void PlayerClickedRestart()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        ResetStage();
    }

    public void PlayerClickedLeave()
    {
        AdventureMap.Instance.SetMapToWorld();
        gameObject.SetActive(false);
    }

    private IEnumerator HandleDefeat()
    {
        resultUI.ShowDefeat();
        if (isAutoplay)
        {
            ResetStage();
            yield return new WaitForSeconds(resultScreenTime);
            spawner.ReplaceHeroes();
            currentRoundIndex--;
            BeginNextRound();

        }
    }

    private IEnumerator HandleVictory()
    {
        //onStageWon?.Invoke(resultScreenTime);

        resultUI.ShowVictory();
        if (isAutoplay)
        {
            ResetStage();
            yield return new WaitForSeconds(resultScreenTime);
            spawner.ReplaceHeroes();
            BeginNextRound();
        }
       
    }
    private bool IsRoundOver()
    {
        return unitsByTeam[Team.Enemy].Count == 0 || unitsByTeam[Team.Player].Count == 0;
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
        DestroyUnits(allUnits);
        resultUI.Close();
        StartStage(currentStage);
    }

    private void DestroyUnits(IEnumerable<UnitBrain> brains)
    {
        foreach (UnitBrain ctx in brains)
        {
            Destroy(ctx.gameObject);
        }
    }

    public void IncreaseStageExperience()
    {
        stageExperience += Mathf.FloorToInt((currentRoundIndex + 1) * currentStage.experienceMultiplierPerRound  * (1+ DataHelpers.GetSumOfStats(UnitStat.ExperienceGain)));
        onStageExperienceChanged?.Invoke(stageExperience);
    }

    public void DecreaseStageExperience(int amount)
    {
        stageExperience -= amount;
        onStageExperienceChanged?.Invoke(stageExperience);
    }


}
