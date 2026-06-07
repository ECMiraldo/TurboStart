using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public struct SpawnIntent
{
    public EnemyDataSO monster;
    public int count;
}
public class CombatSpawner : MonoBehaviour
{

    private Dictionary<HeroData, Vector2> heroStartPositions = new();
    public void SpawnRound(RoundDefinitionSO round)
    {
        var intents = RoundBuilder.Build(round);

        foreach (var intent in intents)
        {
            for (int i = 0; i < intent.count; i++)
            {
                SpawnEnemy(intent.monster, round.difficultyMultiplier);
            }
        }
    }

    public bool PlaceHero(HeroData data, Vector2 worldPos, bool savePos = true)
    {
        var cell = GridSystem.Instance.WorldToGrid(worldPos);
        if (GridSystem.Instance.CanPlaceFootprint(cell, data.template.footprintOffsets))
        {
            var go = Instantiate(data.template.prefab, transform);
            GridComponent unitGridComponent = go.GetComponent<GridComponent>();
            HeroStatsComponent heroStatsComponent = go.GetComponent<HeroStatsComponent>();
            heroStatsComponent.SetHeroData(data);
            GridSystem.Instance.PlaceUnit(unitGridComponent, cell, data.template.footprintOffsets);
            var targetWorldPos = GridSystem.Instance.GridToWorld(cell);
            go.transform.position = targetWorldPos;
            if (savePos) heroStartPositions[data] = worldPos;
            return true;
        }
        return false;

    }

    private void SpawnEnemy(EnemyDataSO data, float difficulty, Vector2Int? cell = null)
    {
        var go = Instantiate(data.prefab, transform);

        Vector2Int anchorCell;
        if (cell == null) anchorCell = FindFreeSpawnCell(data);
        else anchorCell = cell.Value;

        GridComponent unitGridComponent = go.GetComponent<GridComponent>();
        GridSystem.Instance.PlaceUnit(unitGridComponent, anchorCell, data.footprintOffsets);
        var targetWorldPos = GridSystem.Instance.GridToWorld(anchorCell);
        go.transform.position = targetWorldPos;
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
            if (GridSystem.Instance.CanPlaceFootprint(cell,enemy.footprintOffsets))
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

    public void ReplaceHeroes()
    {
        foreach (HeroData data in heroStartPositions.Keys)
        {
           
            PlaceHero(data, heroStartPositions[data], false);
        }

    }
}
