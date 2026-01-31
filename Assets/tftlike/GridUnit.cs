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

    private Vector2Int? desiredStepThisTick;
    public bool HasDesiredStep => desiredStepThisTick.HasValue;

    private Vector3 targetWorldPos;


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

    public void SetDesiredStep(Vector2Int step) => desiredStepThisTick = step;
    public Vector2Int ConsumeDesiredStep()
    {
        var step = desiredStepThisTick.Value;
        desiredStepThisTick = null;
        anchorCell = step;
        return step;
    }

    public void ClearDesiredStep()
    {
        desiredStepThisTick = null;
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
