using UnityEngine.Pool;
using UnityUtils;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections.Generic;

namespace ObjectPooling
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [SerializeField] private List<ObjectPoolSettings> objectsToPool = new();
        private SerializedDictionary<ObjectPoolSettings, IObjectPool<GameObject>> pools = new();

        protected override void Awake()
        {
            base.Awake();
            CreatePools();
        }
        private void CreatePools()
        {
            foreach (ObjectPoolSettings poolSettings in objectsToPool)
            {
                IObjectPool<GameObject> pool;
                pool = new ObjectPool<GameObject>(
                    poolSettings.Create,
                    poolSettings.OnGet,
                    poolSettings.OnRelease,
                    poolSettings.OnDestroyPoolObject,
                    true,
                    poolSettings.defaultCapacity,
                    poolSettings.maxCapacity
                );
                pools.Add(poolSettings, pool);
            }
        }

        public static GameObject Spawn(ObjectPoolSettings settings) => Instance.pools[settings]?.Get();
        public static void ReturnToPool(GameObject go)
        {
            if (go.TryGetComponent(out IPooledObject pooledObject))
            {
                Instance.pools[pooledObject.PoolSettings]?.Release(go);
            }
        }
    }
}

