using UnityEngine;

public class AdventureMap : UIPanelController 
{
    public static AdventureMap Instance;

    [field: SerializeField] public AdventureMapStageDetails stageDetails { get; private set; }
    [field: SerializeField] public CombatSessionManager combatManager { get; private set; }
    [field: SerializeField] public GameObject heroBoard { get; private set; }
    [field: SerializeField] public GameObject stageTracker { get; private set; }
    //TEMPORARY SHIT
    [field: SerializeField] public GameObject background { get; private set; }

    private StageDefinitionSO currentStage;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        stageTracker.SetActive(false);
    }

    private void OnDisable()
    {
        stageDetails.gameObject.SetActive(false);
        stageTracker.SetActive(false);

    }

    public void StageButtonClicked(StageDefinitionSO stageSO)
    {
        stageDetails.ShowDetails(stageSO);
        currentStage = stageSO;
    }

    public void ShowHUD()
    {
        heroBoard.SetActive(true);
    }

    public void StartStage()
    {   
        combatManager.StartStage(currentStage);

        heroBoard.SetActive(true);
        stageTracker.SetActive(true);
        background.SetActive(true);
        Close();
    }

    public void LeaveStage()
    {
        stageTracker.SetActive(false);
        Open();
    }


}
