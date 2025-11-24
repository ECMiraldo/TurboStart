using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<Transform> enemySpots;
    [SerializeField] private List<Transform> heroSpots;
    [SerializeField] private float unitTurnInterval = 1;

    public List<HeroBattleController> heroes { get; private set; }
    public List<EnemyBattleController> enemies { get; private set; }

    private WaitForSeconds unitTurnYield;

    private void Awake()
    {
        unitTurnYield = new WaitForSeconds(unitTurnInterval);
    }

    public void StartStage(HeroTeam team, AdventureMapStageSO stage) => StartCoroutine(BattleCoroutine(team, stage));
    private IEnumerator BattleCoroutine(HeroTeam team, AdventureMapStageSO stage)
    {
        int currentFight = 0;
        heroes = SpawnHeroes(team);

        Logger.LogBattle($"Stage {stage.name} Started");


        yield return new WaitForSeconds(1);
        while (currentFight < stage.numberFights)
        {
            Logger.LogBattle($"Fight {currentFight} Started");


            enemies = SpawnEnemies(stage.BuyEncounter(currentFight));
            List<Unit> turnOrders = GetTurnOrder(enemies, heroes);
            yield return new WaitForSeconds(1);


            while (enemies.Any((x) => x.unitVitals.currentHealth > 0) && 
                  heroes.Any((x) => x.unitVitals.currentHealth > 0))

            {
                

                int currentUnit = 0;
                while (currentUnit < turnOrders.Count)
                {
                    Logger.LogBattle($"Turn {currentUnit} for {turnOrders[currentUnit].name} Started");

                    yield return turnOrders[currentUnit].DoMove();

                    currentUnit++;

                    yield return unitTurnYield;
                }
            }


            if (heroes.All((x) => x.unitVitals.currentHealth == 0))
            {
                Logger.LogBattle("Player Team Died");
                HandlePlayerTeamDied();
                yield break;
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
            if (heroes[i] == null) continue;
            HeroTemplateSO heroTemplate = heroes[i].SO();
            HeroBattleController controller = heroTemplate.InstantiateBattlePrefab(heroSpots[i]);
            controller.SetData(heroes[i]);
            controller.SetBattleManager(this);
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
            enemyBattleController.SetBattleManager(this);
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
