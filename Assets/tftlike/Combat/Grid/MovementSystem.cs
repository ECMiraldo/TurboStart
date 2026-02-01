using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Tick-based movement executor that consumes paths and manages reservations
public class MovementSystem : MonoBehaviour
{
    public static MovementSystem Instance { get; private set; }

    [SerializeField] private int ticksPerSecond = 4;
    
    public GridPathfinder gridPathFinder { get; private set; } 
    
    private GridSystem grid;
    private int currentTick;
    private float tickTimer;

    private readonly List<UnitContext> activeUnits = new();
    private Dictionary<Team, List<UnitBrain>> unitsByTeam = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        unitsByTeam[Team.Enemy] = new ();
        unitsByTeam[Team.Player] = new ();
        unitsByTeam[Team.Neutral] = new ();
    }

    private void Start()
    {
        grid = GridSystem.Instance;
        gridPathFinder = new(grid);
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        float tickInterval = 1f / ticksPerSecond;

        while (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;
            Tick();
        }
    }

    public void RegisterUnit(UnitBrain unit, Team team)
    {
        if (!activeUnits.Contains(unit.Context))
        {
            activeUnits.Add(unit.Context);
            unitsByTeam[team].Add(unit);
        }

    }

    public void UnregisterUnit(UnitBrain unit, Team team)
    {
        activeUnits.Remove(unit.Context);
        unitsByTeam[team].Remove(unit);
    }

    private void Tick()
    {
        currentTick++;

        // Step 1: clear expired reservations
        CleanupReservations();

        foreach (var ctx in activeUnits)
        {
            var unit = ctx.Grid;
            if (!unit.HasDesiredStep)
                continue;

            Vector2Int next = unit.ConsumeDesiredStep();

            if (grid.CanReserveFootprint(
                next,
                unit.footprintOffsets,
                unit,
                currentTick + 1))
            {
                grid.ReserveFootprint(
                    next,
                    unit.footprintOffsets,
                    unit,
                    currentTick + 1);

                grid.MoveUnit(unit, next);
                unit.SyncVisualPosition(grid.GridToWorld(next));
            }
        }
    }

    private void CleanupReservations()
    {
        foreach (var cell in grid.GetAllCells())
        {
            if (cell.ReservedBy != null && cell.ReservedUntilTick <= currentTick)
            {
                cell.ReservedBy = null;
                cell.ReservedUntilTick = 0;
            }
        }
    }

    #region SpatialQueries
    public UnitContext FindClosestUnit(Vector2Int currentPos, Team whatTeam)
    {
        UnitBrain closest = null;
        int bestDist = int.MaxValue;

        foreach (var unit in unitsByTeam[whatTeam].Concat(unitsByTeam[Team.Neutral]))
        {
            if (unit == this) continue;
            if (unit.Context.Health.IsDead) continue;
            GridComponent gridUnit = unit.Context.Grid;

            int dist = Mathf.Abs(gridUnit.anchorCell.x - currentPos.x)
            + Mathf.Abs(gridUnit.anchorCell.y - currentPos.y);

            if (dist < bestDist)
            {
                bestDist = dist;
                closest = unit;
            }
        }
        if (closest == null) return null;
        return closest.Context;
    }


    #endregion
}
