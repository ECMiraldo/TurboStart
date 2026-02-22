using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// Core grid authority. Owns occupancy & reservations.
public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance { get; private set; }

    [Header("Grid Size")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Transform grid_0_pos;

    [field: SerializeField] public List<GridCell> debugCells => cells.Values.ToList();

    public readonly Dictionary<Vector2Int, GridCell> cells = new();
    public readonly Dictionary<GridComponent, List<Vector2Int>> unitOccupiedCells = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Initialize(width, height);
    }

    #region Initialization

    public void Initialize(int width, int height)
    {
        cells.Clear();
        unitOccupiedCells.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2Int(x, y);
                cells[pos] = new GridCell(pos);
            }
        }
    }

    #endregion

    #region Cell Queries
    public IEnumerable<GridCell> GetAllCells() => cells.Values;

    public bool IsInsideGrid(Vector2Int cell)
        => cells.ContainsKey(cell);

    #endregion

    #region Footprint Validation

    public bool CanPlaceFootprint(Vector2Int anchorCell,Vector2Int[] footprintOffsets,GridComponent ignoreUnit = null)
    {
        foreach (var offset in footprintOffsets)
        {
            var cellPos = anchorCell + offset;

            if (!IsInsideGrid(cellPos))
                return false;

            var cell = cells[cellPos];

            if (cell.Occupant != null && cell.Occupant != ignoreUnit)
                return false;

            if (cell.ReservedBy != null && cell.ReservedBy != ignoreUnit)
                return false;
        }
        return true;
    }

    #endregion

    #region Placement / Removal

    public bool PlaceUnit(GridComponent unit, Vector2Int anchorCell, Vector2Int[] footprintOffsets)
    {
        print($"Placing unit   '''   Unit occupied cells {unitOccupiedCells}");
        if (!CanPlaceFootprint(anchorCell, footprintOffsets))
            return false;

        var occupied = new List<Vector2Int>();

        foreach (var offset in footprintOffsets)
        {
            var cellPos = anchorCell + offset;
            cells[cellPos].Occupant = unit;
            occupied.Add(cellPos);
        }

        unitOccupiedCells[unit] = occupied;
        unit.anchorCell = anchorCell;
        return true;
    }

    public void RemoveUnit(GridComponent unit)
    {
        print($"Removing unit   '''   Unit occupied cells {unitOccupiedCells}");

        if (!unitOccupiedCells.TryGetValue(unit, out var occupied))
            return;

        foreach (var cell in occupied)
        {
            if (cells[cell].Occupant == unit)
                cells[cell].Occupant = null;
        }

        ClearReservations(unit);
        unitOccupiedCells.Remove(unit);
    }

    #endregion

    #region Movement

    public void MoveUnit(GridComponent unit, Vector2Int newAnchorCell)
    {
        RemoveUnit(unit);
        PlaceUnit(unit, newAnchorCell, unit.footprintOffsets);
    }

    #endregion

    #region Reservations

    public bool CanReserveFootprint(GridComponent reservingUnit, Vector2Int cell, Vector2Int[] footprintOffsets, int untilTick)
    {
        foreach (var offset in footprintOffsets)
        {
            Logger.LogGrid($"{reservingUnit.gameObject.name} trying to reserve cell {cell} ");
            var cellPos = cell + offset;
            var offsetCell = cells[cellPos];

            if (offsetCell.Occupant != null && offsetCell.Occupant != reservingUnit)
                return false;

            if (offsetCell.ReservedBy != null &&
                offsetCell.ReservedBy != reservingUnit &&
                offsetCell.ReservedUntilTick >= untilTick)
                return false;
        }
        return true;
    }

    public void ReserveFootprint(GridComponent reservingUnit, Vector2Int cell, Vector2Int[] footprintOffsets, int untilTick)
    {
        foreach (var offset in footprintOffsets)
        {
            Logger.LogGrid($"{reservingUnit.gameObject.name} reerverd cell {cell} ");
            var cellPos = cell + offset;
            var offsetCell = cells[cellPos];
            offsetCell.ReservedBy = reservingUnit;
            offsetCell.ReservedUntilTick = untilTick;
        }
    }

    public void ClearReservations(GridComponent unit)
    {
        foreach (var cell in cells.Values)
        {
            if (cell.ReservedBy == unit)
            {
                cell.ReservedBy = null;
                cell.ReservedUntilTick = 0;
            }
        }
    }

    #endregion

    #region Neighborhood / Range

    public IEnumerable<Vector2Int> GetNeighbors(Vector2Int cell, bool diagonals = false)
    {
        static IEnumerable<Vector2Int> Cardinal()
        {
            yield return Vector2Int.left;
            yield return Vector2Int.right;
            yield return Vector2Int.up;
            yield return Vector2Int.down;
        }

        static IEnumerable<Vector2Int> Diagonal()
        {
            yield return new Vector2Int(1, -1);
            yield return new Vector2Int(-1, -1);
            yield return new Vector2Int(1, 1);
            yield return new Vector2Int(-1, 1);
        }

        foreach (var dir in Cardinal())
        {
            var n = cell + dir;
            if (IsInsideGrid(n)) yield return n;
        }

        if (!diagonals) yield break;

        foreach (var dir in Diagonal())
        {
            var n = cell + dir;
            if (IsInsideGrid(n)) yield return n;
        }
    }
    #endregion

    #region World Conversion

    public Vector3 GridToWorld(Vector2Int cell)
    {
        return grid_0_pos.position +
          new Vector3(cell.x * cellSize, cell.y * cellSize, 0f);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        var local = worldPos - grid_0_pos.position;

        return new Vector2Int(
            Mathf.RoundToInt(local.x / cellSize),
            Mathf.RoundToInt(local.y / cellSize)
        );
    }

    #endregion
}
