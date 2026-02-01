using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private CombatSpawner spawner;
    [SerializeField] private RoundController roundController;

    private StageDefinitionSO currentStage;
    private int currentRoundIndex;

    public void StartStage(StageDefinitionSO stage)
    {
        currentStage = stage;
        currentRoundIndex = 0;

        CombatSession.Instance.SetState(CombatState.StageSetup);
        BeginNextRound();
    }

    public void BeginNextRound()
    {
        if (currentRoundIndex >= currentStage.rounds.Count)
        {
            CombatSession.Instance.SetState(CombatState.StageComplete);
            return;
        }

        var round = currentStage.rounds[currentRoundIndex];
        currentRoundIndex++;

        roundController.StartRound(round);
    }
}
