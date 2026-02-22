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
                new Vector2Int(13, 14),   // X
                new Vector2Int(2, 5)       // Y
            )
        },
        {
            Positioning.FLANK,
            new PositionSpan(
                new Vector2Int(14, 16),
                new Vector2Int(0, 2),      // lower lane
                new Vector2Int(6, 8)       // upper lane
            )
        },
        {
            Positioning.MIDDLE,
            new PositionSpan(
                new Vector2Int(14, 16),
                new Vector2Int(2, 5)
            )
        },
        {
            Positioning.BACK_LINE,
            new PositionSpan(
                new Vector2Int(17, 17),
                new Vector2Int(0, 8)
            )
        }
    };
}
