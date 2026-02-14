using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRoundTracker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private RectTransform roundsContainer;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Image roundIconPrefab;

    private readonly List<Image> roundIcons = new();

    private StageDefinitionSO currentStage;
    private int currentRoundIndex;

    private void OnEnable()
    {
        CombatSessionManager.onStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        CombatSessionManager.onStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(CombatState state)
    {
        var session = CombatSessionManager.Instance;

        switch (state)
        {
            case CombatState.StageSetup:
                gameObject.SetActive(true);
                BuildForStage(session);
                break;

            case CombatState.PreRound:
                UpdateRoundFill(session);
                break;

            case CombatState.StageComplete:
                slider.value = slider.maxValue;
                break;
        }
    }

    // ---------- Stage Setup ----------

    private void BuildForStage(CombatSessionManager session)
    {
        currentStage = session.currentStage; // see note below
        currentRoundIndex = 0;

        ClearIcons();

        int roundCount = currentStage.rounds.Count;

        slider.minValue = 0;
        slider.maxValue = roundCount;
        slider.value = 0;

        CreateIcons(currentStage);
        RecalculateSpacing(roundCount);
    }

    private void ClearIcons()
    {
        foreach (var icon in roundIcons)
            Destroy(icon.gameObject);

        roundIcons.Clear();
    }

    private void CreateIcons(StageDefinitionSO stage)
    {
        foreach (var round in stage.rounds)
        {
            var img = Instantiate(roundIconPrefab, roundsContainer);
            img.sprite = round.icon; // from RoundDefinitionSO
            img.color = Color.gray;  // inactive look
            roundIcons.Add(img);
        }
    }

    private void RecalculateSpacing(int count)
    {
        if (count <= 1)
        {
            layoutGroup.spacing = 0;
            return;
        }

        float containerWidth = roundsContainer.rect.width;
        float iconWidth = roundIconPrefab.rectTransform.rect.width;

        float totalIconsWidth = iconWidth * count;
        float spacing = (containerWidth - totalIconsWidth) / (count - 1);

        layoutGroup.spacing = Mathf.Max(0, spacing);
    }

    // ---------- Round Progress ----------

    private void UpdateRoundFill(CombatSessionManager session)
    {
        currentRoundIndex = session.currentRoundIndex; // see note below

        slider.value = currentRoundIndex;

        for (int i = 0; i < roundIcons.Count; i++)
        {
            roundIcons[i].color = i < currentRoundIndex
                ? Color.white   // completed
                : Color.gray;   // upcoming
        }
    }
}
