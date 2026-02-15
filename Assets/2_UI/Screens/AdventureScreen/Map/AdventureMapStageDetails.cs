using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMapStageDetails : MonoBehaviour
{
    [Header("Ui Refs")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private List<Image> heroButtonImages;
    [SerializeField] private GameObject fightRenderTexture;
 
    private StageDefinitionSO stage;

    public void ShowDetails(StageDefinitionSO stage)
    {
        this.gameObject.SetActive(true);

    }
}
