using Persistence;
using TMPro;
using UnityEngine;



public class AdventureMap : MonoBehaviour 
{
    public static AdventureMap Instance;

    [field: SerializeField] public MapLocationDetails stageDetails { get; private set; }
    [field: SerializeField] public Transform mapContent { get; private set; }
    [field: SerializeField] public Transform mapViewport { get; private set; }
    [field: SerializeField] public GameObject mapContainer { get; private set; }

    //TEMPORARY SHIT
    [field: SerializeField] public CombatSessionManager combatManager { get; private set; }
    [field: SerializeField] public GameObject heroBoard { get; private set; }
    [field: SerializeField] public GameObject stageTracker { get; private set; }
    [field: SerializeField] public GameObject background { get; private set; }
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

}
