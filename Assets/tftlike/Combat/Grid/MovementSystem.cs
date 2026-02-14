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

    private List<UnitContext> activeUnits = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        grid = GridSystem.Instance;
        gridPathFinder = new(grid);
    }

    public void SetActiveUnits(IEnumerable<UnitBrain> units)
    {
        var activeUnits = units.Select(u => u.Context).Distinct().ToList();
        
        foreach (var unit in activeUnits)
        {
            if (!this.activeUnits.Contains(unit))
                this.activeUnits.Add(unit);
        }
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

    private void Tick()
    {
        currentTick++;


        var duplicates = activeUnits
    .GroupBy(u => u)
    .Where(g => g.Count() > 1)
    .Select(g => g.Key);

        foreach (var d in duplicates)
            Debug.LogError($"Duplicate active unit: {d.Grid.gameObject.name}");

        // Step 1: clear expired reservations
        CleanupReservations();

        Dictionary<UnitContext, Vector2Int> intents = new();

        foreach (var ctx in activeUnits)
        {
            if (gridPathFinder.TryGetNextStep(ctx.Grid, ctx.Grid.anchorCell, ctx.Grid.desiredCell, out Vector2Int step))
            {
                if (grid.CanReserveFootprint(ctx.Grid, step, ctx.Grid.footprintOffsets, currentTick))
                {
                    grid.ReserveFootprint(ctx.Grid, step, ctx.Grid.footprintOffsets, currentTick);
                    intents[ctx] = step;
                }
            }
        }
        foreach (var ctx in intents.Keys)
        {
            Logger.LogGrid($"moving {ctx.Grid.gameObject.name} to reserverd cell {intents[ctx]} ");
            
            grid.MoveUnit(ctx.Grid, intents[ctx]);
            ctx.Grid.MoveToStep(intents[ctx]);
        }
    }

    private void CleanupReservations()
    {
        foreach (var cell in grid.GetAllCells())
        {
            if (cell.ReservedBy != null && cell.ReservedUntilTick <= currentTick)
            {
                Logger.LogGrid($"cell {cell.Position} cleaned reservation ");

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

        foreach (var unit in CombatSessionManager.Instance.unitsByTeam[whatTeam].Concat(
                                        CombatSessionManager.Instance.unitsByTeam[Team.Neutral]))
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
