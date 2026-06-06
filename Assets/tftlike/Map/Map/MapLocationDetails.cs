using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLocationDetails : MonoBehaviour
{
    [SerializeField] private MapPlayerParty playerParty;

    [Header("Ui Refs")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [field: SerializeField] public Transform lootTypeImagesParent { get; private set; }
    [SerializeField] private Button actionButton;

    public List<Image> lootTypeImages = new();
    private void Awake()
    {
        for (int i = 0; i < lootTypeImagesParent.childCount; i++)
        {
            lootTypeImages.Add(lootTypeImagesParent.GetChild(i).GetComponent<Image>());
        }
    }

    private void OnDisable()
    {
        actionButton.onClick.RemoveAllListeners();
        lootTypeImagesParent.gameObject.SetActive(false);
    }

    public void ShowLocationDetails(MapLocationButton location)
    {
        //check if player is in this location
        stageNameText.text = location.mapLocationSO.displayName;
        descriptionText.text = location.mapLocationSO.description;
        this.gameObject.SetActive(true);

        if (playerParty.destination != null) return; //currentlyTravelling

        if (playerParty.currentLocation == location) //enter
            actionButton.onClick.AddListener(() => location.Enter()); //travelling
        else 
            actionButton.onClick.AddListener(() => playerParty.CommitTravel(location)); //travelling
    }
  
}
