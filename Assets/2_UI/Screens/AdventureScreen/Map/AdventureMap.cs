using UnityEngine;

public class AdventureMap : MonoBehaviour
{
    public static AdventureMap Instance;

    [field: SerializeField] public AdventureMapStageDetails stageDetails { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void StageButtonClicked(AdventureMapStageSO stageSO)
    {
        stageDetails.ShowDetails(stageSO);
    }


}
