using UnityEngine;

[CreateAssetMenu(menuName = "ObjectPoolSettings")]
public class ObjectPoolSettings : ScriptableObject
{
    [field: SerializeField] public GameObject prefab { get; private set; }
    [field: SerializeField] public int prewarmAmount { get; private set; }

}


