using UnityEngine;
using System.Collections;

public class RoundController : MonoBehaviour
{
    [SerializeField] private StageController stageController;
    [SerializeField] private CombatSpawner spawner;
    [SerializeField] private float preRoundDelay = 1.5f;
    [SerializeField] private float postRoundDelay = 2f;

    private RoundDefinitionSO currentRound;

    public void StartRound(RoundDefinitionSO round)
    {
        currentRound = round;
        StartCoroutine(RoundFlow());
    }

    private IEnumerator RoundFlow()
    {
        // PRE-ROUND
        CombatSession.Instance.SetState(CombatState.PreRound);
        UIEvents.ShowPreRound(currentRound);
        yield return new WaitForSeconds(preRoundDelay);

        // SPAWN
        CombatSession.Instance.SetState(CombatState.Spawning);
        spawner.SpawnRound(currentRound);

        yield return null; // one frame safety

        // COMBAT
        CombatSession.Instance.SetState(CombatState.Combat);

        yield return new WaitUntil(AllEnemiesDead);

        // POST-ROUND
        CombatSession.Instance.SetState(CombatState.PostRound);
        UIEvents.ShowRewards(currentRound);
        yield return new WaitForSeconds(postRoundDelay);

        UIEvents.HideRewards();

        // NEXT ROUND
        stageController.BeginNextRound();
    }

    private bool AllEnemiesDead()
    {
        return CombatTracker.Instance.ActiveEnemies == 0;
    }
}
