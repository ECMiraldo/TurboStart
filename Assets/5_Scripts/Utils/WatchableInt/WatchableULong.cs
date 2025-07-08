using System;
using UnityEngine;

[Serializable]
public class WatchableULong 
{
    public event Action<ulong> onChanged;

    [SerializeField] private ulong value;

    public ulong Value
    {
        get { return value; }
        set { 
            this.value = (ulong)value;
            onChanged?.Invoke(value);
            }
    }
}
