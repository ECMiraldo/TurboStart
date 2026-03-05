using System.Collections.Generic;
using UnityEngine;



public enum Positioning
{
    FRONT_LINE,
    FLANK,
    MIDDLE,
    BACK_LINE,
}

public struct PositionSpan
{
    public Vector2Int xRange;          // inclusive
    public List<Vector2Int> yRanges;    // allows split lanes (flanks)

    public PositionSpan(
        Vector2Int xRange,
        params Vector2Int[] yRanges)
    {
        this.xRange = xRange;
        this.yRanges = new List<Vector2Int>(yRanges);
    }
}


public static class PositioningTable
{

    public static readonly Dictionary<Positioning, PositionSpan> Spans
        = new()
    {
        {
            Positioning.FRONT_LINE,
            new PositionSpan(
                new Vector2Int(11, 12),   // X
                new Vector2Int(1, 4)       // Y
            )
        },
        {
            Positioning.FLANK,
            new PositionSpan(
                new Vector2Int(12, 13),
                new Vector2Int(0, 1),      // lower lane
                new Vector2Int(4, 5)       // upper lane
            )
        },
        {
            Positioning.MIDDLE,
            new PositionSpan(
                new Vector2Int(13, 14),
                new Vector2Int(1, 4)
            )
        },
        {
            Positioning.BACK_LINE,
            new PositionSpan(
                new Vector2Int(14, 15),
                new Vector2Int(0, 5)
            )
        }
    };
}
