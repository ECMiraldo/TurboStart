using System;
using UnityEngine;

// Internal cell model
[Serializable]
public class GridCell
{
    [field: SerializeField] public Vector2Int Position { get; }
    [field: SerializeField] public GridComponent Occupant { get; set; }
    [field: SerializeField] public GridComponent ReservedBy { get; set; }
    [field: SerializeField] public int ReservedUntilTick { get; set; }
    public bool IsFree => Occupant == null && ReservedBy == null;

    public GridCell(Vector2Int pos)
    {
        Position = pos;
    }
}
