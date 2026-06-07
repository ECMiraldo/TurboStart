using System.Collections.Generic;
using UnityEngine;

public class MapStageLocationButton : MapLocationButton
{
    [field: SerializeField] public StageDefinitionSO stageDefinitionSO;
    [field: SerializeField] public CombatSessionManager combatSessionManager;
    public override MapLocationSO mapLocationSO => stageDefinitionSO;

    private void OnEnable()
    {
        //make it inactive in case no hero is at a given level or any other condition here
    }

    public override void Enter()
    {
        AdventureMap.Instance.SetMapToWindow();
        combatSessionManager.gameObject.SetActive(true);
        combatSessionManager.StartStage(stageDefinitionSO);
    }

    protected override void ShowDetails()
    {
        base.ShowDetails();
        ShowLootTypes();
    }

    private void ShowLootTypes()
    {
        List<Sprite> lootTypeSprites = stageDefinitionSO.GetPossibleDropIcons();

        int i = 0;
        for (i = 0; i < lootTypeSprites.Count; i++)
        {
            locationDetails.lootTypeImages[i].sprite = lootTypeSprites[i];
            if (i > locationDetails.lootTypeImages.Count - 1)
                Debug.LogError("Too many loot types for stage:" + stageDefinitionSO.name);
        }

        for (int j = i; j < locationDetails.lootTypeImages.Count; j++)
        {
            locationDetails.lootTypeImages[j].enabled = false;
        }
        locationDetails.lootTypeImagesParent.gameObject.SetActive(true);

    }

}
