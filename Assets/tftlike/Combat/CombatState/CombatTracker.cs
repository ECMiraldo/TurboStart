using UnityEngine;

public class CombatTracker : MonoBehaviour
{
    public static CombatTracker Instance { get; private set; }

    public int ActiveEnemies { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy()
    {
        ActiveEnemies++;
    }

    public void UnregisterEnemy()
    {
        ActiveEnemies--;
    }
}
