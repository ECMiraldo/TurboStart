using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class SceneObjectPool : MonoBehaviour
{
    [field: SerializeField] private List<ScenePoolSettings> poolSettings;

    private static ObjectPool pool;

    private void Awake()
    {
        pool = new(poolSettings, this);
    }

    public static IPooledObject Spawn(ObjectPoolSettings settings, Vector3? position = null) => pool.Spawn(settings, position);
    public static void Despawn(IPooledObject gameObject) => pool.Despawn(gameObject);
}
