using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMapStageDetails : MonoBehaviour
{
    [Header("Ui Refs")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private Transform lootTypeImagesParent;
    [SerializeField] private GameObject fightRenderTexture;
 
    private StageDefinitionSO stage;
    private List<Image> lootTypeImages = new();
    private void Awake()
    {   
        for (int i = 0; i < lootTypeImagesParent.childCount; i++)
        {
            lootTypeImages.Add(lootTypeImagesParent.GetChild(i).GetComponent<Image>());
        }
    }

    public void ShowDetails(StageDefinitionSO stage)
    {
        this.stage = stage;
        this.gameObject.SetActive(true);
        ShowLootTypes();
    }

    private void ShowLootTypes()
    {
        List<Sprite> lootTypeSprites = stage.GetPossibleDropIcons();

        int i = 0;
        for (i = 0; i < lootTypeSprites.Count; i++)
        {
            lootTypeImages[i].sprite = lootTypeSprites[i];
            if (i > lootTypeImages.Count - 1)
                Debug.LogError("Too many loot types for stage:" + stage.name);
        }

        for (int j = i; j < lootTypeImages.Count; j++)
        {
            lootTypeImages[j].enabled = false;
        }
    }
}
