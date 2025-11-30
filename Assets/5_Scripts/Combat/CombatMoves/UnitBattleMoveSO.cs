using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public enum BattleMoveTarget : byte
{
    self = 0,
    teamFront = 1,
    teamBack = 2,
    wholeTeam = 3,
    EnemyFront = 4,
    EnemyBack = 5,
    EnemyTeam = 6,
}



[CreateAssetMenu(menuName = "Combat/Battle Move")]
public class UnitBattleMoveSO : ScriptableObject
{
    [field: SerializeField] public string moveName { get; private set; }
    [field: SerializeField] public Sprite moveIcon { get; private set; }

    [TextArea]
    [field: SerializeField] public string moveDescription { get; private set; }
    [field: SerializeField] public GameObject moveController { get; private set; }


    [field: SerializeField] public AnimationNames animationName { get; private set; }
    [field: SerializeField] public BattleMoveTarget target { get; private set; }
    [field: SerializeField] public float moveTime { get; private set; }


    public IEnumerable<Unit> GetTargets(Unit unit, bool isHero)
    {
        if (isHero) return GetTargetsForHeroTeam(unit);
        else return GetTargetsForEnemyTeam(unit);
    }
    private IEnumerable<Unit> GetTargetsForHeroTeam(Unit hero)
    {
        switch (target)
        {
            case BattleMoveTarget.self: return new List<Unit> { hero };

            case BattleMoveTarget.teamFront:
                return new List<Unit> { hero.battleManager.heroes.First((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.teamBack:
                return new List<Unit> { hero.battleManager.heroes.Last((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.wholeTeam:
                return hero.battleManager.heroes;

            case BattleMoveTarget.EnemyFront:
                return new List<Unit> { hero.battleManager.enemies.First((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.EnemyBack:
                return new List<Unit> { hero.battleManager.enemies.Last((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.EnemyTeam:
                return hero.battleManager.enemies;
        }
        return null;
    }


    private IEnumerable<Unit> GetTargetsForEnemyTeam(Unit enemy)
    {
        switch (target)
        {
            case BattleMoveTarget.self: return new List<Unit> { enemy };

            case BattleMoveTarget.teamFront:
                return new List<Unit> { enemy.battleManager.enemies.First((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.teamBack:
                return new List<Unit> { enemy.battleManager.enemies.Last((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.wholeTeam:
                return enemy.battleManager.enemies;

            case BattleMoveTarget.EnemyFront:
                return new List<Unit> { enemy.battleManager.heroes.First((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.EnemyBack:
                return new List<Unit> { enemy.battleManager.heroes.Last((x) => x.unitVitals.currentHealth > 0) };

            case BattleMoveTarget.EnemyTeam:
                return enemy.battleManager.heroes;
        }
        return null;
    }




}

 
