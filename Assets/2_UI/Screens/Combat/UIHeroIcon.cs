using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHeroIcon : MonoBehaviour
{
    [field: SerializeField] public Image icon { get; private set; }
    [field: SerializeField] public Slider levelSlider { get; private set; }
    [field: SerializeField] public Button levelUpButton { get; private set; }
    [field: SerializeField] public TextMeshProUGUI levelText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI costText { get; private set; }
    [field: SerializeField] public UiDragger dragger { get; private set; }
    [field: SerializeField] public HeroData heroData { get; private set; }

    private HeroStatsComponent heroStats;

    private bool isSpawned = false;
    public void SetData(HeroData data)
    {
        heroData = data;
        icon.sprite = data.template.icon;
        levelText.text = "1";
        costText.text = data.template.GetRequiredExpForLevel(2).ToString();
        HandleExperienceChange(CombatSessionManager.Instance.stageExperience);
    }

    private void OnEnable()
    {
        dragger.enabled = true;
        dragger.onEndDrag += OnEndDrag;
        CombatSessionManager.onStageExperienceChanged += HandleExperienceChange;
    }
    private void OnDisable()
    {
        dragger.onEndDrag -= OnEndDrag;
        CombatSessionManager.onStageExperienceChanged -= HandleExperienceChange;
    }
    private void OnEndDrag(PointerEventData data)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(data.position);
        isSpawned = CombatSessionManager.Instance.spawner.PlaceHero(heroData, worldPos);
        dragger.enabled = !isSpawned;

    }

    public void OnClick()
    {
        if (!isSpawned) return;
        heroStats = CombatSessionManager.Instance.GetHeroStatsByData(heroData);
        if (heroStats == null) return;
        int cost = heroData.template.GetRequiredExpForLevel(heroStats.stageLevel);
        CombatSessionManager.Instance.DecreaseStageExperience(cost);
        heroStats.LevelUp();
        levelText.text = heroStats.stageLevel.ToString();
        costText.text = heroData.template.GetRequiredExpForLevel(heroStats.stageLevel).ToString();
    }

    private void HandleExperienceChange(int exp)
    {
        if (!isSpawned)
        {
            levelUpButton.interactable = false;
            return;
        }
        levelUpButton.interactable = exp >= heroData.template.GetRequiredExpForLevel(heroStats.stageLevel);
    }
  


}
