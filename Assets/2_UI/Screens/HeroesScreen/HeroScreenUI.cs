using UnityEngine;
using Persistence;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class HeroScreenUI : MonoBehaviour
{
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

    private void Awake()
    {
        heroList = GameManager.ProfileData.heroes;
    }
    private void Start()
    {
        SelectHero(0);
    }

    public void OnEnable()
    {
        InstantiateHeroCards();
    }

    public void NextHero()
    {
        if (currentHero + 1 == heroList.Count) currentHero = 0;
        else currentHero++;
        SelectHero(currentHero);
        heroNameText.text = heroList[currentHero].name;

    }

    public void PrevHero()
    {
        if (currentHero - 1 < 0) currentHero = heroList.Count - 1;
        else currentHero--;
        heroNameText.text = heroList[currentHero].name;
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
            img.sprite = h.Sprite;
            heroIcons.Add(h, go);
        }
    }

    private void SelectHero(int idx)
    {
        currentHero = idx;
        heroNameText.text = heroList[currentHero].name;

        statsUI.UpdateStats(heroList[currentHero]);
    }

    private void SelectHero(HeroData hero) => SelectHero(heroList.FindIndex((h) => h == hero));
 }
