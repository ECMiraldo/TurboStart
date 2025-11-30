using TMPro;
using UnityEngine;

public class HeroStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;




    public void UpdateStats(HeroData hero)
    {
        healthText.text = hero.vitals.currentHealth + "/" + hero.attributes.stats[UnitStat.Health].ToInt();
        manaText.text = hero.vitals.currentMana + "/" + hero.attributes.stats[UnitStat.Mana].ToInt();
    }

    
}
