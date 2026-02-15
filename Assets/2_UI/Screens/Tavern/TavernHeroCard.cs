using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TavernHeroCard : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI timeRemaining;

    private Tavern tavern;
    private TavernHero tavernHero;
  

    public void Init(Tavern tavern, TavernHero hero)
    {
        this.tavern = tavern;
        this.toggle.group = tavern.heroListToggleGroup;
        this.toggle.onValueChanged.AddListener(val => OnClick());
        this.tavernHero = hero;
        this.icon.sprite = hero.hero.template.mainSprite;
        this.nameText.text = hero.hero.name;
       // this.timeRemaining.text = TickManager.Instance.ticksToSeconds(hero.hero.Action.ticksRemaining).ToString();
        this.costText.text = hero.cost.ToString();
    }

    private void Update()
    {
       // this.timeRemaining.text = TickManager.Instance.ticksToSeconds(tavernHero.hero.Action.ticksRemaining).ToString();
    }

    public void OnClick()
    {
        tavern.SelectHero(tavernHero);
    }
}
