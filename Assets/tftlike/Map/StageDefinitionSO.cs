using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combat/Stage")]
public class StageDefinitionSO : MapLocationSO
{
    public int requiredLevel;
    public List<RoundDefinitionSO> rounds;
    public LootTable stageLootTable;
    public List<Sprite> GetPossibleDropIcons()
    {
        List<Sprite> result = new List<Sprite>();
        foreach (ItemSO item in stageLootTable.lootDict.Keys)
        {
            if (item is WeaponSO weapon)
            {
                if (Database.weaponTypeIcons.TryGetValue(weapon.WeaponType, out Sprite sprite))
                {
                    if (!result.Contains(sprite)) result.Add(sprite);
                }
            }

            else if (item is EquipmentSO equip)
            {
                if (Database.armorTypeIcons[equip.armorType].TryGetValue(equip.Slot, out Sprite sprite))
                {
                    if (!result.Contains(sprite)) result.Add(sprite);
                }
            }
        }
        return result;  
    }
}
