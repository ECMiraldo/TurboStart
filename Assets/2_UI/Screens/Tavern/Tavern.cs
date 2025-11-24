using Persistence;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tavern : MonoBehaviour
{

    [SerializeField] private Transform heroCardPrefab;

    [Header("UiRefs")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button hireButton;
    [SerializeField] public ToggleGroup heroListToggleGroup { get; private set; }
    [SerializeField] private Transform heroListContent;


    ProfileData profileData;
    TavernHero selectedHero;
    TavernData tavernData;
    private Dictionary<TavernHero, TavernHeroCard> heroCards = new();

    private void Start()
    {
        profileData = SaveLoadSystem.Instance.data;
        tavernData = profileData.tavernData;

        tavernData.availableHeroes.RemoveAll(
            (x) => 
                        x.hero == null
                    || x.hero.Action == null 
                    || x.hero.Action.currentTick == x.hero.Action.durationInTicks
                    );


        if (tavernData.availableHeroes.Count < 3)
        {
            tavernData.AddHero();
            tavernData.AddHero();
        }

        foreach (TavernHero th in tavernData.availableHeroes)
        {
            InstantiateHeroCard(th);
        }
        SelectHero(tavernData.availableHeroes[0]);
    }


   
    private void InstantiateHeroCard(TavernHero hero)
    {
        TavernHeroCard heroCard = Instantiate(heroCardPrefab, heroListContent).GetComponent<TavernHeroCard>();
        heroCard.Init(this, hero);
        heroCards.Add(hero, heroCard);
    }

    public void SelectHero(TavernHero tavernHero)
    {
        selectedHero = tavernHero;
        heroNameText.text = tavernHero.hero.name;
        costText.text = tavernHero.cost.ToString();
        itemIcon.sprite = tavernHero.hero.Sprite;
        hireButton.interactable = profileData.gold >= tavernHero.cost;
    }

    public void HireHero()
    {

        profileData.gold -= selectedHero.cost;
        profileData.heroes.Add(selectedHero.hero);
        tavernData.availableHeroes.Remove(selectedHero);
      

        Destroy(heroCards[selectedHero].gameObject);
        heroCards.Remove(selectedHero);

        if (tavernData.availableHeroes.Count > 0)
            SelectHero(tavernData.availableHeroes[0]);
    }


}
