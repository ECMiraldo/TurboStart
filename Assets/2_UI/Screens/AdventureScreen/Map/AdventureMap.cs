using UnityEngine;

public class AdventureMap : MonoBehaviour
{
    public static AdventureMap Instance;

    [field: SerializeField] public AdventureMapStageDetails stageDetails { get; private set; }
    [field: SerializeField] public CombatSessionManager combatManager { get; private set; }
    [field: SerializeField] public GameObject fightUi { get; private set; }

    private StageDefinitionSO currentStage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        stageDetails.gameObject.SetActive(false);
    }

    public void StageButtonClicked(StageDefinitionSO stageSO)
    {
        stageDetails.ShowDetails(stageSO);
        currentStage = stageSO;
    }

    public void StartStage()
    {   
        combatManager.gameObject.SetActive(true);
        combatManager.StartStage(currentStage);
        fightUi.SetActive(true);
        this.gameObject.SetActive(false);

    }


}
