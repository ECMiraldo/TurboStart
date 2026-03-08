using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ResourceData
{
    [field: JsonIgnore] public static event Action<long> onGoldChanged;
    public CharacterAttribute goldMultiplier { get; private set; } = new CharacterAttribute(1);
    public CharacterAttribute dropChanceMultiplier { get; private set; } = new CharacterAttribute(1);
    private long _gold = 0;
    public long gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            onGoldChanged?.Invoke(_gold);
        }
    }

    public void AddGold(int amount, bool withModifiers = true) 
    {
        if (withModifiers) amount = Mathf.FloorToInt(amount * goldMultiplier.Value);
        gold += amount;
    }





}

