using UnityEngine;

public class IPooledObjectImpl : MonoBehaviour, IPooledObject
{
    [field: SerializeField] public ObjectPoolSettings poolSettings { get; private set; }
}
