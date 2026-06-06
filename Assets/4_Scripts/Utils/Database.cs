using System;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    public static readonly Dictionary<string, HeroTemplateSO> heroTemplates = new();
    public static readonly Dictionary<string, ItemSO> items = new();
    public static readonly Dictionary<ArmorType, Dictionary<EquipmentSlot, Sprite>> armorTypeIcons = new();
    public static readonly Dictionary<WeaponType,Sprite > weaponTypeIcons = new();
    
    static Database()
    {
        LoadHeroTemplates();
        LoadItems();
        LoadArmorTypes();
        LoadWeaponTypes();
    }

    static void LoadHeroTemplates()
    {
        var allTemplates = Resources.LoadAll<HeroTemplateSO>("Heroes");
        foreach (var template in allTemplates)
        {
            heroTemplates.Add(template.id, template);
        }
    }

    static void LoadItems()
    {
        var allItems = Resources.LoadAll<ItemSO>("Items");
        foreach (var item in allItems)
        {
            items.Add(item.id, item);
        }
    }    
    
    static void LoadArmorTypes()
    {
        ArmorType[] allArmorTypes = (ArmorType[])System.Enum.GetValues(typeof(ArmorType));
        EquipmentSlot[] allSlots = (EquipmentSlot[])System.Enum.GetValues(typeof(EquipmentSlot));
        foreach (var type in allArmorTypes)
        {
            armorTypeIcons.Add(type, new Dictionary<EquipmentSlot, Sprite>());
            foreach (var slot in allSlots)
            {
                try
                {
                    Sprite sprite = Resources.Load<Sprite>($"Items/CategoryIcons/{type}-{slot}");
                    if (sprite != null) armorTypeIcons[type][slot] = sprite;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
           
        }
    }

    static void LoadWeaponTypes()
    {
        WeaponType[] allArmorTypes = (WeaponType[])System.Enum.GetValues(typeof(WeaponType));
        foreach (var type in allArmorTypes)
        {
            weaponTypeIcons.Add(type, null);
            try
            {
                Sprite sprite = Resources.Load<Sprite>($"Items/CategoryIcons/{type.ToString()}");
                if (sprite != null) weaponTypeIcons[type] = sprite;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

}
