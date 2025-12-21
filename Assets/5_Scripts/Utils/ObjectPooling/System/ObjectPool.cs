using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic ObjectPool controller that must be attached to some MonoBehaviour.
/// Receives a list of ObjectPoolSettings that is used to prewarm the pool at creation.
/// Can be used with objects that are not on the list, but those will NOT be prewarmed.
/// </summary>
public class ObjectPool
{
    private readonly Dictionary<ObjectPoolSettings, List<IPooledObject>> activeObjects;
    private readonly Dictionary<ObjectPoolSettings, List<IPooledObject>> inactiveObjects;
    private readonly Dictionary<ObjectPoolSettings, Transform> transforms;
    private readonly MonoBehaviour monoBehaviour;

    public ObjectPool(IEnumerable<ObjectPoolSettings> settings, MonoBehaviour parent)
    {
        monoBehaviour = parent;
        activeObjects = new();
        inactiveObjects = new();
        transforms = new();


        foreach (ObjectPoolSettings poolSettings in settings)
        {
            CreateObjectPool(poolSettings);
            //prewarms inactives
            for (int i = 0; i < poolSettings.prewarmAmount; i++)
            {
                IPooledObject newObject = GameObject.Instantiate(poolSettings.prefab, transforms[poolSettings]).GetComponent<IPooledObject>();
                inactiveObjects[poolSettings].Add(newObject);
                newObject.gameObject.SetActive(false);
            }
        }
    }

    private void CreateObjectPool(ObjectPoolSettings settings)
    {
        //Sets transform parent in hierarchy
        Transform transformParent = new GameObject(settings.name).transform;
        transformParent.SetParent(monoBehaviour.transform);
        transformParent.localPosition = Vector3.zero;
        transformParent.localScale = Vector3.one;
        //initializes lists
        transforms.Add(settings, transformParent);
        inactiveObjects.Add(settings, new());
        activeObjects.Add(settings, new());

    }

    public IPooledObject Spawn(ObjectPoolSettings settings, Vector3? position)
    {
        if (!activeObjects.ContainsKey(settings)) CreateObjectPool(settings);

        Vector3 finalPos = position.HasValue ? position.Value : Vector3.zero;

        IPooledObject maybeObject = inactiveObjects[settings].Count > 0 ? inactiveObjects[settings][0] : null;
        if (maybeObject == null)
        {
            maybeObject = GameObject.Instantiate(settings.prefab, finalPos, Quaternion.identity, transforms[settings]).GetComponent<IPooledObject>();
            activeObjects[settings].Add(maybeObject);
        }
        else
        {
            inactiveObjects[settings].RemoveAt(0);
            activeObjects[settings].Add(maybeObject);
            maybeObject.transform.position = finalPos;
            maybeObject.gameObject.SetActive(true);
        }
        return maybeObject;
    }

    public void Despawn(IPooledObject pooledObject)
    {
        ObjectPoolSettings settings = pooledObject.poolSettings;
        if (activeObjects[settings].Contains(pooledObject)) activeObjects[settings].Remove(pooledObject);
        inactiveObjects[settings].Add(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }

    public void DespawnAll()
    {
        foreach (var pool in activeObjects.Values)
        {

            while (pool.Count > 0)
            {
                Despawn(pool[0]);
            }
        }
    }

}
