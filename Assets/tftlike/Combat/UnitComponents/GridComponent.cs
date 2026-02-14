using UnityEngine;
using System.Collections.Generic;

// Base unit class for grid-based movement & tactics
public class GridComponent : MonoBehaviour
{
    [Header("Grid")]
    [field: SerializeField] public Vector2Int anchorCell;
    [field: SerializeField] public Vector2Int starterCell;

    [Tooltip("Footprint offsets relative to anchor cell")]
    [field: SerializeField] public Vector2Int[] footprintOffsets { get; private set; } = { Vector2Int.zero };

    [Header("Visuals")]
    [SerializeField] private CombatVisuals combatVisuals;
    [SerializeField] private float moveLerpSpeed = 8f;

    public Vector2Int desiredCell { get; set; }
    private Vector3 targetWorldPos;


    private void Start()
    {
        targetWorldPos = GridSystem.Instance.GridToWorld(anchorCell);
        starterCell = anchorCell;
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

    public void JumpToCell(Vector2Int cell)
    {
        GridSystem.Instance.RemoveUnit(this);
        GridSystem.Instance.PlaceUnit(this, cell, footprintOffsets);
        anchorCell = cell;
        targetWorldPos = GridSystem.Instance.GridToWorld(cell);
        desiredCell = cell;
        combatVisuals.UpdateVisuals(cell, cell);
    }

    public void WalkTowardsCell(Vector2Int cell)
    {
        desiredCell = cell;
    }

    public void ResetMovement()
    {
        desiredCell = anchorCell;
    }

    public void MoveToStep(Vector2Int cell)
    {
        combatVisuals.UpdateVisuals(anchorCell, cell);
        targetWorldPos = GridSystem.Instance.GridToWorld(cell);
        anchorCell = cell;
        desiredCell = cell;
    }

    #endregion
}


// Extension for debug access
public static class GridUnitDebugExtensions
{
    public static IEnumerable<Vector2Int> DebugPath(this GridComponent unit)
    {
        var field = typeof(GridComponent)
            .GetField("plannedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field == null) yield break;

        var queue = field.GetValue(unit) as Queue<Vector2Int>;
        if (queue == null) yield break;

        foreach (var step in queue)
            yield return step;
    }
}
