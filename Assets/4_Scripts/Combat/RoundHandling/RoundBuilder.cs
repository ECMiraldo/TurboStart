using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundBuilder
{
    public static List<SpawnIntent> Build(RoundDefinitionSO round)
    {
        var plans = new List<SpawnIntent>();
        int remainingPower = round.totalPowerBudget;

        var pool = round.monsterPool
            .OrderBy(m => m.powerCost)
            .ToList();

        while (remainingPower > 0)
        {
            var candidate = PickMonster(pool, remainingPower);
            if (candidate == null)
                break;

            int maxCount = remainingPower / candidate.powerCost;
            int count = Random.Range(1, maxCount + 1);

            remainingPower -= count * candidate.powerCost;

            plans.Add(new SpawnIntent
            {
                monster = candidate,
                count = count,
            });
        }

        return plans;
    }

    private static EnemyDataSO PickMonster(List<EnemyDataSO> pool,int remainingPower)
    {
        var valid = pool.Where(m => m.powerCost <= remainingPower).ToList();
        if (valid.Count == 0) return null;

        // bias toward weaker units
        return valid[Random.Range(0, Mathf.Min(3, valid.Count))];
    }

  
}
