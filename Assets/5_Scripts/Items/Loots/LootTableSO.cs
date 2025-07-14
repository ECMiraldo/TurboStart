//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using ObjectPooling;
//using Persistence;

//[Serializable]
//public struct LootData
//{
//    [SerializeField] public ItemSO item;
//    [SerializeField] public Vector2Int quantity;
//    [SerializeField] public float chance;
//    [SerializeField] public ObjectPoolSettings poolSettings;
//}

//[CreateAssetMenu(menuName = "Enemies/Loot Table")]
//public class LootTableSO : ScriptableObject
//{
//    [SerializeField]
//    private List<LootData> lootData;


//    public List<LootData> RollLoot()
//    {
//        List<LootData> list = new();

//        foreach (LootData ld in lootData)
//        {
//            float rand = UnityEngine.Random.Range(0, 100.0f);
//            if (rand <= (ld.chance * SaveLoadSystem.Instance.CurrentProfile.profileSettings.dropRateMult)) list.Add(ld);
//        }
//        return list;
//    }

//    public void DropItems(Vector3 pos, List<LootData> loot = null)
//    {
//        if (loot == null) loot = RollLoot();
//        foreach (LootData ld in loot)
//        {
//            Debug.Log($"Spawning loot at {pos}");
//            LootController drop = GlobalObjectPool.Spawn(ld.poolSettings).GetComponent<LootController>();
//            drop.gameObject.SetActive(true);
//            drop.transform.position = pos;
//            Rigidbody body = drop.GetComponent<Rigidbody>();
//            drop.transform.position = pos;
//            body.MovePosition(pos);
//            int qnt = ld.quantity.x == ld.quantity.y ? ld.quantity.x : UnityEngine.Random.Range(ld.quantity.x, ld.quantity.y + 1);
//            drop.SetItem(ld.item, qnt);
//        }

//    }

//}



