using UnityEngine;
using System.Collections.Generic;

// Base unit class for grid-based movement & tactics
public class GridUnit : MonoBehaviour
{
    [Header("Grid")]
    [field: SerializeField] public Vector2Int anchorCell;

    [Tooltip("Footprint offsets relative to anchor cell")]
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; } = { Vector2Int.zero };

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveLerpSpeed = 8f;

    private readonly Queue<Vector2Int> plannedPath = new();
    private Vector2Int? confirmedStep;
    private Vector3 targetWorldPos;

    public bool HasPlannedPath => plannedPath.Count > 0;
    public bool HasConfirmedStep => confirmedStep.HasValue;

    private void Start()
    {
        anchorCell = GridSystem.Instance.WorldToGrid(transform.position);
        GridSystem.Instance.PlaceUnit(this, anchorCell, footprintOffsets);
        targetWorldPos = GridSystem.Instance.GridToWorld(anchorCell);
        transform.position = targetWorldPos;
    }

    private void OnDestroy()
    {
        if (GridSystem.Instance != null)
            GridSystem.Instance.RemoveUnit(this);
    }

    private void Update()
    {
        // Smooth visual interpolation
        transform.position = Vector3.Lerp(
            transform.position,
            targetWorldPos,
            Time.deltaTime * moveLerpSpeed);
    }

    #region Path Control

    public void SetPath(List<Vector2Int> path)
    {
        plannedPath.Clear();

        // Skip first cell (current position)
        for (int i = 1; i < path.Count; i++)
            plannedPath.Enqueue(path[i]);
    }

    public Vector2Int PeekNextPathCell()
    {
        return plannedPath.Peek();
    }

    public void ConfirmStep()
    {
        confirmedStep = plannedPath.Peek();
    }

    public Vector2Int ConsumeConfirmedStep()
    {
        var step = confirmedStep.Value;
        if (anchorCell.x != step.x) spriteRenderer.flipX = anchorCell.x > step.x;
        plannedPath.Dequeue();
        confirmedStep = null;
        anchorCell = step;
        return step;
    }
    
    public void ClearPath()
    {
        plannedPath.Clear();
        confirmedStep = null;
    }

    public void OnMovementBlocked()
    {
        // Simple behavior: clear path and wait
        plannedPath.Clear();
        confirmedStep = null;
    }

    #endregion

    #region Visual Sync

    public void SyncVisualPosition(Vector3 worldPos)
    {
        targetWorldPos = worldPos;
    }

    #endregion
}


// Extension for debug access
public static class GridUnitDebugExtensions
{
    public static IEnumerable<Vector2Int> DebugPath(this GridUnit unit)
    {
        var field = typeof(GridUnit)
            .GetField("plannedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field == null) yield break;

        var queue = field.GetValue(unit) as Queue<Vector2Int>;
        if (queue == null) yield break;

        foreach (var step in queue)
            yield return step;
    }
}
