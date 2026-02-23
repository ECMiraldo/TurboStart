using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TavernHeroCard : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    private Tavern tavern;
    private (HeroData, long) heroAndCost;
  

    public void Init((HeroData, long) heroAndCost)
    {
        this.heroAndCost = heroAndCost;
        this.icon.sprite = heroAndCost.Item1.template.icon;
        this.nameText.text = "add hero names dumbass";
        this.costText.text = heroAndCost.Item2.ToString();
    }

}
