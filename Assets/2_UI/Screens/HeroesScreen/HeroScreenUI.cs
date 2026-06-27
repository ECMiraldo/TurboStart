using UnityEngine;
using Persistence;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class HeroScreenUI : UIPanelController
{
    public event Action<HeroData> onHeroChanged;

    [Header("UiRefs")]
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private Transform heroScrollContent;
    [SerializeField] private GameObject heroScrollIconPrefab;
    [SerializeField] private ToggleGroup heroScrollToggleGroup;

    [Header("Handlers")]
    [SerializeField] private HeroStatsUI statsUI;

    private List<HeroData> heroList;
    private Dictionary<HeroData, GameObject> heroIcons = new();
    private int currentHero;
    protected override void Start()
    {
        base.Start();
        SelectHero(0);
    }

    protected void OnEnable()
    {
        heroList = SaveLoadSystem.Instance.data.heroes;
        InstantiateHeroCards();
    }

    public void NextHero()
    {
        if (currentHero + 1 == heroList.Count) currentHero = 0;
        else currentHero++;
        SelectHero(currentHero);

    }

    public void PrevHero()
    {
        if (currentHero - 1 < 0) currentHero = heroList.Count - 1;
        else currentHero--;
        SelectHero(currentHero);
    }

    private void InstantiateHeroCards()
    {
        foreach (HeroData h in heroList)
        {
            if (heroIcons.ContainsKey(h) && heroIcons[h] != null) continue;

            GameObject go = Instantiate(heroScrollIconPrefab, heroScrollContent);
            Toggle tg = go.GetComponent<Toggle>();
            tg.group = heroScrollToggleGroup;
            tg.onValueChanged.AddListener((x) => SelectHero(h));
            Image img = go.GetComponent<Image>();
            img.sprite = h.template.icon;
            heroIcons.Add(h, go);
        }
    }

    private void SelectHero(int idx)
    {
        currentHero = idx;
        heroNameText.text = heroList[currentHero].name;
        statsUI.UpdateStats(heroList[currentHero]);
        onHeroChanged?.Invoke(heroList[currentHero]);
    }

    private void SelectHero(HeroData hero) => SelectHero(heroList.FindIndex((h) => h == hero));

    public HeroData GetCurrentHero() => heroList[currentHero];
 }
