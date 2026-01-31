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

    public bool TryFindPath(
        GridUnit unit,
        Vector2Int start,
        Vector2Int goal,
        out List<Vector2Int> path,
        int maxIterations = 1000)
    {
        path = null;

        var openSet = new PriorityQueue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        var gScore = new Dictionary<Vector2Int, int>
        {
            [start] = 0
        };

        openSet.Enqueue(start, Heuristic(start, goal));

        int iterations = 0;

        while (openSet.Count > 0 && iterations++ < maxIterations)
        {
            var current = openSet.Dequeue();

            if (current == goal)
            {
                path = ReconstructPath(cameFrom, current);
                return true;
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
                    int fScore = tentativeG + Heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return false;
    }

    public bool TryFindPathToRange(
        GridUnit unit,
        Vector2Int start,
        Vector2Int target,
        int range,
        out List<Vector2Int> path)
    {
        path = null;

        var validTargets = new HashSet<Vector2Int>();
        foreach (var cell in grid.GetCellsInRange(target, range))
        {
            if (grid.CanPlaceFootprint(cell, unit.footprintOffsets, unit))
                validTargets.Add(cell);
        }

        if (validTargets.Count == 0)
            return false;

        var openSet = new Queue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var visited = new HashSet<Vector2Int>();

        openSet.Enqueue(start);
        visited.Add(start);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (validTargets.Contains(current))
            {
                path = ReconstructPath(cameFrom, current);
                return true;
            }

            foreach (var neighbor in grid.GetNeighbors(current))
            {
                if (visited.Contains(neighbor)) continue;
                if (!grid.CanPlaceFootprint(neighbor, unit.footprintOffsets, unit)) continue;

                visited.Add(neighbor);
                cameFrom[neighbor] = current;
                openSet.Enqueue(neighbor);
            }
        }

        return false;
    }

    private static int Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return dx + dy + (dx > dy ? 0 : 1);
    }

    private static List<Vector2Int> ReconstructPath(
        Dictionary<Vector2Int, Vector2Int> cameFrom,
        Vector2Int current)
    {
        var path = new List<Vector2Int> { current };

        while (cameFrom.TryGetValue(current, out var prev))
        {
            current = prev;
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}
