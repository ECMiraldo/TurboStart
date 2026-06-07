using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PolygonUI : Graphic
{
    public List<Vector2> points = new List<Vector2>();

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 3) return;

        for (int i = 0; i < points.Count; i++)
        {
            vh.AddVert(points[i], color, Vector2.zero);
        }

        for (int i = 1; i < points.Count - 1; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }
        SetAllDirty();
    }
}