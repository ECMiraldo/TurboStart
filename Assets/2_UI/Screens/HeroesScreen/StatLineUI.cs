using UnityEngine;
using TMPro;
public class StatLineUI : MonoBehaviour
{
    [SerializeField] private HeroScreenUI heroScreenUI;
    [SerializeField] private UnitStat unitStat;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnEnable()
    {
        heroScreenUI.onHeroChanged += UpdateStatLine;
        UpdateStatLine(heroScreenUI.GetCurrentHero());
    }

    private void OnDisable()
    {
        heroScreenUI.onHeroChanged -= UpdateStatLine;
    }

    private void UpdateStatLine(HeroData heroData)
    {
        var stats = heroData.template.GetStats(heroData.level);

        statName.text = unitStat.ToString();
        if (stats.ContainsKey(unitStat))
        {
            statValue.text = stats[unitStat].ToString(2);
        }
        else
        {
            statValue.text = "0";
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (statName == null) statName = transform.Find("StatName").GetComponent<TextMeshProUGUI>();
        if (statValue == null) statValue = transform.Find("StatValue").GetComponent<TextMeshProUGUI>();
        statName.text = unitStat.ToString();
        statValue.text = "100";
    }
    #endif
   
}
