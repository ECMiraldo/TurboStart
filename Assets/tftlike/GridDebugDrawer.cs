using UnityEngine;
// ------------------------------------------------------------
// DEBUG UTILITIES
// ------------------------------------------------------------

// Draws grid, occupancy, reservations, and paths
public class GridDebugDrawer : MonoBehaviour
{
    [SerializeField] private Color gridColor = Color.gray;
    [SerializeField] private Color occupiedColor = Color.red;
    [SerializeField] private Color reservedColor = Color.yellow;
    [SerializeField] private Color pathColor = Color.cyan;

    private GridSystem grid;

    private void Awake()
    {
        grid = GridSystem.Instance;
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        float size = 1f;

        foreach (var cell in grid.GetAllCells())
        {
            Vector3 world = grid.GridToWorld(cell.Position);

            Gizmos.color = gridColor;
            Gizmos.DrawWireCube(world, Vector3.one * size);

            if (cell.Occupant != null)
            {
                Gizmos.color = occupiedColor;
                Gizmos.DrawCube(world, Vector3.one * (size * 0.5f));
            }
            else if (cell.ReservedBy != null)
            {
                Gizmos.color = reservedColor;
                Gizmos.DrawCube(world, Vector3.one * (size * 0.4f));
            }
        }

        // Draw unit paths
        foreach (var unit in FindObjectsByType<GridUnit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            var pos = grid.GridToWorld(unit.anchorCell);
            Gizmos.color = pathColor;

            foreach (var step in unit.DebugPath())
            {
                var next = grid.GridToWorld(step);
                Gizmos.DrawLine(pos, next);
                pos = next;
            }
        }
    }
}
