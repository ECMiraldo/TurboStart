using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectPool : MonoBehaviour
{
    [field: SerializeField] private List<ObjectPoolSettings> poolSettings;

    private static  ObjectPool pool;

    private void Awake()
    {
        pool = new(poolSettings, this);
    }

    public static IPooledObject Spawn(ObjectPoolSettings settings, Vector3? position = null) => pool.Spawn(settings, position);
    public static void Despawn(IPooledObject gameObject) => pool.Despawn(gameObject);
    public static void UnloadObjects() => pool.DespawnAll();
}

