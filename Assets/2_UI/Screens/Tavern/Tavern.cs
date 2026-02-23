using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tavern : MonoBehaviour
{
    [Header("UiRefs")]
    [SerializeField] private GameObject playerHeroCardPrefab;
    [SerializeField] private Transform playerHeroListContent;
    [SerializeField] private List<TavernHeroCard> forHireHeroCards;
    [SerializeField] private TextMeshProUGUI timeRemaining;

    public readonly Dictionary<HeroData, GameObject> heroIcons = new();
    private PersistentData data;
    private TavernData tavernData;

    private void Awake()
    {
        data = SaveLoadSystem.Instance.data;
        tavernData = data.tavernData;
    }

    private void OnEnable()
    {
        TickManager.OnTick += OnTick;
        CheckRefresh();
        UpdateRemainingTime();
        RefreshPlayerHeroList();

    }
    private void OnTick()
    {
        CheckRefresh();
        UpdateRemainingTime();
    }

    private void OnDisable()
    {
        TickManager.OnTick -= OnTick;
    }

    private void CheckRefresh()
    {
        if (tavernData.nextRefresh <= data.lastTickTime) RefreshList();
    }

    private void RefreshList()
    {
        tavernData.availableHeroes.Clear();
        for (int i = 0; i < tavernData.nCardsShown; i++)
        {
            forHireHeroCards[i].gameObject.SetActive(true);
            (HeroData, long) heroCost = tavernData.CreateHero();
            forHireHeroCards[i].Init(heroCost);
        }
        tavernData.nextRefresh = DateTimeOffset.UtcNow.AddHours(tavernData.durationInHours).ToUnixTimeSeconds();
        for (int i = tavernData.nCardsShown; i < forHireHeroCards.Count; i++)
        {
            forHireHeroCards[i].gameObject.SetActive(false);
        }
    }

    private void UpdateRemainingTime()
    {
        timeRemaining.text = Utils.FormatTime(tavernData.nextRefresh - DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    private void RefreshPlayerHeroList()
    {
        foreach (HeroData h in data.heroes)
        {
            if (heroIcons.ContainsKey(h) && heroIcons[h] != null) continue;

            GameObject go = Instantiate(playerHeroCardPrefab, playerHeroListContent);
           // tg.onValueChanged.AddListener((x) => SelectHero(h));
            Image img = go.GetComponent<Image>();
            img.sprite = h.template.icon;
            heroIcons.Add(h, go);
        }
    }


    public void HireHero()
    {

        //profileData.gold -= selectedHero.cost;
        //profileData.heroes.Add(selectedHero.hero);
        //tavernData.availableHeroes.Remove(selectedHero);
      

        //Destroy(heroCards[selectedHero].gameObject);
        //heroCards.Remove(selectedHero);
    }


}
