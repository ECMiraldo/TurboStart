using System.Collections.Generic;
using UnityEngine;
// Footprint-aware A* pathfinding operating purely on GridSystem
public class GridPathfinder
{
    private readonly GridSystem grid;

    public GridPathfinder(GridSystem grid)
    {
        this.grid = grid;
    }

    public bool TryGetNextStep(
        GridComponent unit,
        Vector2Int start,
        Vector2Int goal,
        out Vector2Int nextStep,
        int maxIterations = 100)
    {
        nextStep = start;

        var openSet = new PriorityQueue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int>
        {
            [start] = 0
        };

        int bestHeuristic = Heuristic(start, goal);
        Vector2Int bestNode = start;

        openSet.Enqueue(start, bestHeuristic);

        int iterations = 0;

        while (openSet.Count > 0 && iterations++ < maxIterations)
        {
            var current = openSet.Dequeue();

            int h = Heuristic(current, goal);
            if (h < bestHeuristic)
            {
                bestHeuristic = h;
                bestNode = current;
            }

            foreach (var neighbor in grid.GetNeighbors(current))
            {
                if (!grid.CanPlaceFootprint(neighbor, unit.footprintOffsets, unit))
                    continue;

                int tentativeG = gScore[current] + 1;

                if (!gScore.TryGetValue(neighbor, out int existingG) || tentativeG < existingG)
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    int f = tentativeG + Heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, f);
                }
            }
        }

        if (bestNode != start)
        {
            nextStep = ReconstructFirstStep(start, bestNode, cameFrom);
            return true;
        }

        return false;
    }

    private static Vector2Int ReconstructFirstStep(
        Vector2Int start,
        Vector2Int end,
        Dictionary<Vector2Int, Vector2Int> cameFrom)
    {
        Vector2Int current = end;

        while (cameFrom.TryGetValue(current, out var prev))
        {
            if (prev == start)
                return current;

            current = prev;
        }

        return start;
    }

    private static int Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        // Chebyshev + slight vertical penalty
        return Mathf.Max(dx, dy) * 10 + dy;
    }

}
