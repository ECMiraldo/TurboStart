using UnityEngine;

public interface IPooledObject
{
    public ObjectPoolSettings poolSettings { get; }
    public GameObject gameObject { get; }
    public Transform transform { get; }
}
