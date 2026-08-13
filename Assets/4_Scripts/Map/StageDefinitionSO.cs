using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Combat/Stage")]
public class StageDefinitionSO : MapLocationSO
{
    public int requiredLevel;
    public int nRounds;
    public SerializedDictionary<int, RoundDefinitionSO> specialRounds;
    public LootTable stageLootTable;

    [Header("Allowed Monsters")]
    public List<EnemyDataSO> monsterPool;

   
    [Header("Rewards")]
    public float experienceMultiplierPerRound = 1.0f;
    public float round0Experience = 10.0f;

    [Header("Scaling")]
    public int round0Budget = 5;
    public float budgetMultiplierPerRound = 1.5f;
    public float enemyRoundMultiplier = 1.5f;

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
