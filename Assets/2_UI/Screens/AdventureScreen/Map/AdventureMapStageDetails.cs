using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMapStageDetails : MonoBehaviour
{
    [Header("Ui Refs")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private List<Image> heroButtonImages;
 

    [Header("Dependencies")]
    [SerializeField] private AdventureMapHeroChoosingScreen heroChoosingScreen;
    [SerializeField] private BattleManager battleConstructor;

    private HeroTeam tempTeam;
    private AdventureMapStageSO stage;

    private void OnDisable()
    {
        tempTeam = null;
        ClearHero(0);
        ClearHero(1);
        ClearHero(2);
    }

    public void ShowDetails(AdventureMapStageSO stageSO)
    {
        gameObject.SetActive(true);
        tempTeam = new HeroTeam();
        stage = stageSO;
    }

    //from ui button
    public void OpenHeroChoosingScreen(int heroSlot)
    {
        heroChoosingScreen.OpenToChooseHero(tempTeam, heroSlot);
    }

    //from ui button
    public void StartStage()
    {
        if (!tempTeam.heroes.Exists((x) => x != -1)) return; //there are no heroes on the team

        battleConstructor.StartStage(tempTeam, stage);
    }


    public void SelectHero(HeroData hero, int slot)
    {
        tempTeam.heroes[slot] = hero.heroId;
        heroButtonImages[slot].sprite = hero.Sprite;
    }

    public void ClearHero(int slot)
    {
        tempTeam.heroes[slot] = -1;
        heroButtonImages[slot].sprite = null;
    }



}
