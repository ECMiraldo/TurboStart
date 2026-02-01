using UnityEngine;

// Internal cell model
public class GridCell
{
    public Vector2Int Position { get; }
    public GridComponent Occupant;
    public GridComponent ReservedBy;
    public int ReservedUntilTick;
    public bool IsFree => Occupant == null && ReservedBy == null;

    public GridCell(Vector2Int pos)
    {
        Position = pos;
    }
}
