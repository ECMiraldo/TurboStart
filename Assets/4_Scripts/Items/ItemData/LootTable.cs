using AYellowpaper.SerializedCollections;
using Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LootTable
{
    [field: SerializeField] public SerializedDictionary<ItemSO, int> lootDict = new();

    public List<ItemSO> GetDrops()
    {
        List<ItemSO> lootList = new();
        foreach (ItemSO item in lootDict.Keys)
        {
            float roll = UnityEngine.Random.Range(0, 101);
            roll *= 1 / DataHelpers.GetPartyStat(PartyStat.DropChance);
            if (roll < lootDict[item]) lootList.Add(item);
        }
        return lootList;
    }



}
