using System.Collections.Generic;
using UnityEngine;
public struct SpawnIntent
{
    public EnemyDataSO monster;
    public int count;
}
public class CombatSpawner : MonoBehaviour
{
    public void SpawnRound(RoundDefinitionSO round)
    {
        var intents = RoundBuilder.Build(round);

        foreach (var intent in intents)
        {
            for (int i = 0; i < intent.count; i++)
            {
                Vector2Int cell = FindFreeSpawnCell(intent.monster);
                SpawnEnemy(intent.monster, cell, round.difficultyMultiplier);
            }
        }
    }

    private void SpawnEnemy(EnemyDataSO data, Vector2Int cell, float difficulty)
    {
        var stats = MonsterScaler.Scale(data, difficulty);
        var go = Instantiate(data.prefab);
        // init grid + stats here
    }

    private Vector2Int FindFreeSpawnCell(EnemyDataSO enemy)
    {
        var span = PositioningTable.Spans[enemy.positioning];
        var candidates = new List<Vector2Int>();

        // Generate candidates
        foreach (var yBand in span.yRanges)
        {
            for (int x = span.xRange.x; x <= span.xRange.y; x++)
            {
                for (int y = yBand.x; y <= yBand.y; y++)
                {
                    candidates.Add(new Vector2Int(x, y));
                }
            }
        }

        // Shuffle (important!)
        Shuffle(candidates);

        // Pick first valid
        foreach (var cell in candidates)
        {
            if (GridSystem.Instance.CanPlaceFootprint(
                cell,
                enemy.footprintOffsets,
                null))
            {
                return cell;
            }
        }

        // Fallback
        Debug.LogWarning($"No free spawn cell for {enemy.name}");
        return new Vector2Int(span.xRange.x, span.yRanges[0].x);
    }

    private static void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
