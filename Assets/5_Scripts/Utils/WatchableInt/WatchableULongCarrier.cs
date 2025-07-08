using UnityEngine;

[CreateAssetMenu (menuName = "Watchables/ULong")]
public class WatchableULongCarrier : ScriptableObject
{
    [field: SerializeField] public WatchableULong watchableULong { get; private set; }
}
