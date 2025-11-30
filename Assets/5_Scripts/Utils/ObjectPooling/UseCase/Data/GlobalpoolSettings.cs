using UnityEngine;

[CreateAssetMenu(menuName = "ObjectPooling/GlobalPoolSettings")]
public class GlobalpoolSettings : ObjectPoolSettings 
{ 
    [field: SerializeField] public bool despawnOnSceneChange { get; private set; }
}


