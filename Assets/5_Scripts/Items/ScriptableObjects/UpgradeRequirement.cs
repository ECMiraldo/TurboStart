using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeRequirement
{
    [field: SerializeField] public List<ItemQuantityPair> items { get; private set; } = new(4);
}

[Serializable]
public class ItemQuantityPair
{
    [field: SerializeField] public int quantity { get; private set; }
    [field: SerializeField] public Item item { get; private set; }
}