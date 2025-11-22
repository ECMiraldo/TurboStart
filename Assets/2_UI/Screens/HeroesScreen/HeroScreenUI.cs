using UnityEngine;
using Persistence;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class HeroScreenUI : MonoBehaviour
{
    [Header("UiRefs")]
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private InventoryUI inventoryUI;

    private List<HeroData> heroList;
    private int currentHero;
    private void Start()
    {
        heroList = GameManager.ProfileData.heroes;
        currentHero = 0;
        heroNameText.text = heroList[0].name;
    }

    public void NextHero()
    {
        if (currentHero + 1 == heroList.Count) currentHero = 0;
        else currentHero++;
        heroNameText.text = heroList[currentHero].name;

    }

    public void PrevHero()
    {
        if (currentHero - 1 < 0) currentHero = heroList.Count - 1;
        else currentHero--;
        heroNameText.text = heroList[currentHero].name;
    }





}
