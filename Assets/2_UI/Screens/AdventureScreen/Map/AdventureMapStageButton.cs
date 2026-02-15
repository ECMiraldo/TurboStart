using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AdventureMapStageButton : MonoBehaviour
{
    [SerializeField] private StageDefinitionSO stageSO;
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private Button button;

    private void Awake()
    {
        stageNameText.text = $"Lvl {stageSO.requiredLevel}. {stageSO.stageName}";
    }

    private void OnEnable()
    {
        //make it inactive in case no hero is at a given level or any other condition here
    }

    public void OnClick()
    {
        AdventureMap.Instance.StageButtonClicked(stageSO);
    }


}
