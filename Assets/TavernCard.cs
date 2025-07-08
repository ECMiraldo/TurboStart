using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TavernCard : MonoBehaviour
{
 
    [SerializeField] private Image icon;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    private Tavern tavern;
    private TavernHero tavernHero;

    public void Init(Tavern tavern, TavernHero hero)
    {
        this.tavern = tavern;
        this.toggle.group = tavern.heroListToggleGroup;
        this.toggle.onValueChanged.AddListener(val => OnClick());
        this.tavernHero = hero;
        this.icon.sprite = hero.Sprite;
        this.nameText.text = hero.Name;
        this.costText.text = hero.cost.ToString();
    }

    public void OnClick()
    {
        tavern.SelectHero(tavernHero);
    }
}
