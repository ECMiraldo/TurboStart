using System.Collections.Generic;
using UnityEngine;


public class MapStageLocationButton : MapLocationButton
{
    [field: SerializeField] public StageDefinitionSO stageDefinitionSO;
    public override MapLocationSO mapLocationSO => stageDefinitionSO;

    private void OnEnable()
    {
        //make it inactive in case no hero is at a given level or any other condition here
    }

    public override void Enter()
    {
        CombatSessionManager.Instance.StartStage(stageDefinitionSO);
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
            stageDetails.lootTypeImages[i].sprite = lootTypeSprites[i];
            if (i > stageDetails.lootTypeImages.Count - 1)
                Debug.LogError("Too many loot types for stage:" + stageDefinitionSO.name);
        }

        for (int j = i; j < stageDetails.lootTypeImages.Count; j++)
        {
            stageDetails.lootTypeImages[j].enabled = false;
        }
        stageDetails.lootTypeImagesParent.gameObject.SetActive(true);

    }

}
