using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public class UIRoundTrackerButtonReference
{
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
}

public class UiRoundTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private  List<UIRoundTrackerButtonReference> roundButtons = new();
    [SerializeField] private Color currentRoundColor = Color.blue;
    [SerializeField] private Color completedRoundColor = Color.green;
    [SerializeField] private Color completedButInactiveRoundColor = Color.yellow;
    [SerializeField] private Color uncompletedRoundColor = Color.gray;
    private void OnEnable()
    {
        CombatSessionManager.onRoundChanged += OnRoundChanged;
        OnRoundChanged(CombatSessionManager.Instance.currentRoundIndex);
    }

    private void OnDisable()
    {
        CombatSessionManager.onRoundChanged -= OnRoundChanged;
    }

    private void OnRoundChanged(int currentRoundIndex)
    {
        if (currentRoundIndex == 0)
        {
            roundButtons[0].Icon.color = currentRoundColor;
            roundButtons[0].Text.text = "1";

            if (CombatSessionManager.Instance.completedRoundIndex + 1 > 0)
                roundButtons[1].Icon.color = completedButInactiveRoundColor;
            else
                roundButtons[1].Icon.color = uncompletedRoundColor;  
            roundButtons[1].Text.text = "2";

            if (CombatSessionManager.Instance.completedRoundIndex + 1 > 1)
                roundButtons[2].Icon.color = completedButInactiveRoundColor;
            else
                roundButtons[2].Icon.color = uncompletedRoundColor;  
            roundButtons[2].Text.text = "3";
        }
        else if (currentRoundIndex == CombatSessionManager.Instance.currentStage.nRounds)
        {
            roundButtons[2].Icon.color = currentRoundColor;
            roundButtons[2].Text.text = (currentRoundIndex + 1).ToString();
            roundButtons[1].Icon.color = completedRoundColor;
            roundButtons[1].Text.text = (currentRoundIndex).ToString();
            roundButtons[0].Icon.color = completedRoundColor;
            roundButtons[0].Text.text = (currentRoundIndex - 1).ToString();
        }
        else
        {
            roundButtons[0].Icon.color = completedRoundColor;
            roundButtons[0].Text.text = currentRoundIndex.ToString();
            roundButtons[1].Icon.color = currentRoundColor;
            roundButtons[1].Text.text = (currentRoundIndex + 1).ToString();
            if (CombatSessionManager.Instance.completedRoundIndex > currentRoundIndex)
                roundButtons[2].Icon.color = completedButInactiveRoundColor;
            else
                roundButtons[2].Icon.color = uncompletedRoundColor;  

            roundButtons[2].Text.text = (currentRoundIndex + 2).ToString();
        }
        for (int i = 0; i < roundButtons.Count; i++)
        {
            int index = i;

            roundButtons[index].Button.onClick.RemoveAllListeners();
            roundButtons[index].Button.interactable =
                index <= CombatSessionManager.Instance.completedRoundIndex + 1;

            roundButtons[index].Button.onClick.AddListener(() =>
                CombatSessionManager.Instance.JumpToRound(
                    int.Parse(roundButtons[index].Text.text) - 1));
        }

    }
}
