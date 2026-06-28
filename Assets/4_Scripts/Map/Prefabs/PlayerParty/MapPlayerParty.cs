using Persistence;
using TMPro;
using UnityEngine;

public class MapPlayerParty : MonoBehaviour
{
    public static MapPlayerParty Instance;

    [field: Header("References")]
    [field: SerializeField] public Transform stagesParent { get; private set; }
    [field: SerializeField] public TextMeshProUGUI travelTimer { get; private set; }

    [field: Header("State")]
    public MapLocationButton currentLocation { get; private set; }
    public MapLocationButton destination { get; private set; }
    private MapData mapData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        mapData = SaveLoadSystem.Instance.data.mapData;
        currentLocation = stagesParent.GetChild(mapData.currentLocationIndex).GetComponent<MapLocationButton>();
    }


    public void CommitTravel(MapLocationButton location)
    {
        destination = location;
        mapData.remainingTravelTicks = location.GetRequiredTicks(currentLocation);
        mapData.destinationLocationIndex = location.transform.GetSiblingIndex();
        travelTimer.gameObject.SetActive(true);
        TickManager.OnTick += OnTick;
    }

    public void OnTick()
    {
        mapData.remainingTravelTicks--;
        travelTimer.text = Utils.FormatTime(mapData.remainingTravelTicks);
        if (mapData.remainingTravelTicks == 0) OnTravelEnded();
    }

    private void OnTravelEnded()
    {
        TickManager.OnTick -= OnTick;
        travelTimer.gameObject.SetActive(false);
        mapData.destinationLocationIndex = -1;
        currentLocation = destination;
        mapData.currentLocationIndex = destination.transform.GetSiblingIndex();
        destination = null;
    }


}
