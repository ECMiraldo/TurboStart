using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private List<Transform> enemySpots;
    [SerializeField] private List<Transform> heroSpots;

    public void StartStage(HeroTeam team, AdventureMapStageSO stage) => StartCoroutine(BattleCoroutine(team, stage));
    private IEnumerator BattleCoroutine(HeroTeam team, AdventureMapStageSO stage)
    {
        int currentFight = 0;
        List<HeroBattleController> heroes = SpawnHeroes(team);

        while (currentFight < stage.numberFights)
        {
            List<EnemyBattleController> enemies = SpawnEnemies(stage.BuyEncounter(currentFight));
            List<Unit> turnOrders = GetTurnOrder(enemies, heroes);
    
            while(enemies.All((x) => x.unitVitals.currentHealth > 0) && 
                  heroes.All((x) => x.unitVitals.currentHealth > 0))

            {
                int currentUnit = 0;
                while (currentUnit < turnOrders.Count)
                {
                    yield return turnOrders[currentUnit].DoMove();
                    currentUnit++;
                }
            }


            if (heroes.Count == 0)
            {
                HandlePlayerTeamDied();
            }
            else currentFight++;

        }
        yield return null;
    }

    #region Spawning 
    private List<HeroBattleController> SpawnHeroes(HeroTeam team)
    {
        List<HeroData> heroes = GetHeroesFromIDs(team);
        List<HeroBattleController> controllers = new();
        for (int i = 0; i < heroes.Count; i++)
        {
            HeroTemplateSO heroTemplate = heroes[i].SO();
            HeroBattleController controller = Instantiate(heroTemplate.prefab, heroSpots[i]).GetComponent<HeroBattleController>();
            controller.SetData(heroes[i]);
            controllers.Add(controller);
        }
        return controllers;
    }

    private List<HeroData> GetHeroesFromIDs(HeroTeam team)
    {
        List<HeroData> heroes = new();
        for (int i = 0; i < team.heroes.Count; i++)
        {
            int id = team.heroes[i];
            if (id == -1) heroes.Add(null);
            else
            {
                HeroData hero = GameManager.ProfileData.heroes.Find((x) => x.heroId == id);
                heroes.Add(hero);
            }
        }
        return heroes;
    }

    private List<EnemyBattleController> SpawnEnemies(List<EnemyDataSO> enemies)
    {
        List<EnemyBattleController> enemyBattleControllers = new ();
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBattleController enemyBattleController = Instantiate(enemies[i].prefab, enemySpots[i]).GetComponent<EnemyBattleController>();
            enemyBattleControllers.Add(enemyBattleController);
        }
        return enemyBattleControllers;
    }
    #endregion

    private List<Unit> GetTurnOrder(List<EnemyBattleController> enemies, List<HeroBattleController> heroes)
    {
        List<Unit> units = new();
        units.AddRange(enemies);
        units.AddRange(heroes);
        units.Sort((x, y) => 1);
        return units;
    }

    private void HandlePlayerTeamDied()
    {

    }

    private void StartNextFight()
    {
        
    }
}
