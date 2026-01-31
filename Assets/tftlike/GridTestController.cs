using UnityEngine;
// ------------------------------------------------------------
// SIMPLE TEST DRIVER
// ------------------------------------------------------------

public class GridTestController : MonoBehaviour
{
    [SerializeField] private GridUnit testUnit;
    [SerializeField] private Vector2Int targetCell;

    private GridPathfinder pathfinder;

    private void Start()
    {
        pathfinder = new GridPathfinder(GridSystem.Instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = pathfinder.TryFindPath(
                testUnit,
                testUnit.anchorCell,
                targetCell,
                out var path
            );

            if (success)
            {
                Debug.Log($"Path found: {path.Count} steps");
                testUnit.SetPath(path);
            }
            else
            {
                Debug.LogWarning(
                    $"NO PATH from {testUnit.anchorCell} to {targetCell}"
                );
            }
        }
    }
}
