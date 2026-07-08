using UnityEngine;
using TMPro;
public class StageExperienceCounter : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI experienceText { get; private set; }

    private void OnEnable()
    {
        CombatSessionManager.onStageExperienceChanged += UpdateExperienceText;
        UpdateExperienceText(CombatSessionManager.Instance.stageExperience);
    }

    private void OnDisable()
    {
        CombatSessionManager.onStageExperienceChanged -= UpdateExperienceText;
    }

    private void UpdateExperienceText(int newExperience)
    {
        experienceText.text = newExperience.ToString();
    }
}