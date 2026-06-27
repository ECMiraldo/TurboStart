using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Ink;

[CreateAssetMenu( menuName = "Items/EquipmentArchetype")]
public class EquipmentArchetypeSO : ScriptableObject
{
    [SerializeField] public int nStats = 2;
    [SerializeField] public List<UnitStat> possibleStats;
    [SerializeField] public int nEffects = 1;
    [SerializeField] public List<ItemEffect> possibleEffects;
     
    public List<AttributeModifier> RollStats(int level, ItemRarity rarity)
    {
        var stats = possibleStats.GetNumberRandomFromList(nStats);
        var attributeModifiers = new List<AttributeModifier>();
        foreach (var stat in stats)
        {
            if (stat == UnitStat.Attack || stat == UnitStat.MagicAttack)
            {
                //Handle flats or multiplier
            }
            AttributeModifier mod = new AttributeModifier(stat, level, ModifierSource.items);
            attributeModifiers.Add(mod);
        }
        return attributeModifiers;

    }

    public List<ItemEffect> RollEffects(int level, ItemRarity rarity)
    {
        return possibleEffects.GetNumberRandomFromList(nEffects);
    }

    public ItemRarity RollRarity(EquipmentSO item)
    {
        float totalWeight = item.rarities.Sum(r => Database.rarityWeights[r]);

        float roll = Random.Range(0f, totalWeight);

        foreach (var rarity in item.rarities)
        {
            roll -= Database.rarityWeights[rarity];

            if (roll <= 0)
                return rarity;
        }

        return item.rarities.First();
    }


}
