using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Zones/Stage")]
public class AdventureMapStageSO : ScriptableObject
{
    [field: SerializeField] public string stageName { get; private set; }
    [field: SerializeField] public int requiredLevel { get; private set; }
    [field: SerializeField] public int numberFights { get; private set; }

    [field: SerializeField] public AnimationCurve enemyBudgetCurve { get; private set; }
    [field: SerializeField] public SerializedDictionary<EnemyDataSO, int> enemyPool { get; private set; }
    [field: SerializeField] public List<EnemyDataSO> possibleBosses { get; private set; }




    //mostly chatGPT thing, needs more attention
    public List<EnemyDataSO> BuyEncounter(int currentFight)
    {
        List<EnemyDataSO> encounter = new List<EnemyDataSO>();

        int remainingCost = Mathf.FloorToInt(enemyBudgetCurve.Evaluate(currentFight));
        int maxEnemies = 3;

        // Convert dictionary to list for easier random selection
        var enemies = enemyPool.ToList();

        while (encounter.Count < maxEnemies && remainingCost > 0)
        {
            // Filter affordable enemies
            var affordable = enemies
                .Where(e => e.Value <= remainingCost)
                .ToList();

            if (affordable.Count == 0)
                break; // Can't afford any more


            // Weighted random by cost — higher cost = higher chance
            int totalWeight = affordable.Sum(e => e.Value);
            int roll = Random.Range(0, totalWeight);
            int cumulative = 0;
            KeyValuePair<EnemyDataSO, int> chosen = default;

            foreach (var e in affordable)
            {
                cumulative += e.Value;
                if (roll < cumulative)
                {
                    chosen = e;
                    break;
                }
            }

            // Add to encounter
            encounter.Add(chosen.Key);
            remainingCost -= chosen.Value;
        }

        return encounter;
    }

}
