using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tavern : MonoBehaviour
{
    [SerializeField] private Transform heroListContent;
    [SerializeField] private Transform heroCardPrefab;
    [SerializeField] public ToggleGroup heroListToggleGroup { get; private set; }

    [Header("UiRefs")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button hireButton;

    TavernHero selectedHero;
    TavernData data;

    private void Start()
    {
        data = SaveLoadSystem.Instance.data.tavernData;

        data.availableHeroes.RemoveAll((x) => x.Action == null || x.Action.currentTick == x.Action.durationInTicks);


        if (data.availableHeroes.Count < 3)
        {
            data.CreateHero(GetRandomSprite());
            data.CreateHero(GetRandomSprite());
        }

        foreach (TavernHero th in data.availableHeroes)
        {
            InstantiateHeroCard(th);
        }
    }

    private static string GetRandomSprite()
    {
        var sprites = Database.Instance.HeroSpriteNames;
        return sprites[Random.Range(0, sprites.Count)];
    }

    private void InstantiateHeroCard(TavernHero hero)
    {
        TavernCard heroCard = Instantiate(heroCardPrefab, heroListContent).GetComponent<TavernCard>();
        heroCard.Init(this, hero);
    }

    public void SelectHero(TavernHero hero)
    {
        selectedHero = hero;
        heroNameText.text = hero.Name;
        costText.text = TickManager.Instance.ticksToSeconds(hero.Action.durationInTicks - hero.Action.currentTick).ToString();
        itemIcon.sprite = hero.Sprite;
    }


}
