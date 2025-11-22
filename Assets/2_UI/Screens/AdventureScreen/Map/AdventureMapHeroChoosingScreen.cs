using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMapHeroChoosingScreen : MonoBehaviour
{
    [SerializeField] private GameObject heroListPrefab;
    [SerializeField] private Transform heroListContent;
    [SerializeField] private AdventureMapStageDetails stageDetailsScreen;

    private List<AdventureMapHeroChoosingCard> heroCardsPool = new List<AdventureMapHeroChoosingCard>();

    HeroData currentSelectedHero;
    HeroTeam tempTeam;
    int tempSlot;

    public void OpenToChooseHero(HeroTeam team, int slot)
    {
        gameObject.SetActive(true);
        tempTeam = team;
        tempSlot = slot;
        currentSelectedHero = null;
        ShowAllHeroes();

        if (tempTeam.heroes[slot] != -1)
        {
            HeroData hero = GameManager.ProfileData.heroes.Find((x) => x.heroId == tempTeam.heroes[slot]);
            int heroIndex = GameManager.ProfileData.heroes.IndexOf(hero);
            ShowHeroDetails(hero);
        }
        else
        {
            ShowHeroDetails(GameManager.ProfileData.heroes[0]);
        }
    }

    public void ChooseHero()
    {
        stageDetailsScreen.SelectHero(currentSelectedHero, tempSlot);
        gameObject.SetActive(false);
    }

    public void ClearHero()
    {
        stageDetailsScreen.ClearHero(tempSlot); 
        gameObject.SetActive(false);
    }

    private void ShowAllHeroes()
    {
        List<HeroData> list = GameManager.ProfileData.heroes;

        list.RemoveAll((x) => tempTeam.heroes.Contains(x.heroId));  


        // Ensure we have enough UI cards
        for (int i = 0; i < list.Count; i++)
        {
            AdventureMapHeroChoosingCard card;
            if (i < heroCardsPool.Count)
            {
                card = heroCardsPool[i];
                card.gameObject.SetActive(true);
            }
            else
            {
                card = Instantiate(heroListPrefab, heroListContent).GetComponent<AdventureMapHeroChoosingCard>();
                heroCardsPool.Add(card);
            }

            SetUpHeroCard(card, list[i]);
        }

        // Hide any unused pooled cards
        for (int i = list.Count; i < heroCardsPool.Count; i++)
        {
            heroCardsPool[i].gameObject.SetActive(false);
        }
    }

    private void SetUpHeroCard(AdventureMapHeroChoosingCard card, HeroData hero)
    {
        Button button = card.GetComponent<Button>();
        card.SetHero(hero);
        button.onClick.RemoveAllListeners(); // clear old listeners
        button.onClick.AddListener(() => ShowHeroDetails(hero));
        //if (hero.Action != null)
        //{
        //    button.interactable = false;
        //}
        //else
        //{
        //    button.interactable = true;
        //    button.onClick.AddListener(() => ShowHeroDetails(hero));
        //}

    }

    private void ShowHeroDetails(HeroData hero)
    {
        currentSelectedHero = hero;
    }
}
