using UnityEngine;
using UnityUtils;

public class ItemFactory : Singleton<ItemFactory>
{
    //public Item CreateItem(int tier)
    //{
    //    return new Item("name", "sprite");
    //}

    private Sprite LoadRandomArmorSprite(int tier, EquipmentSlot slot, ArmorType type, EquipmentRarity rarity, int spriteCount)
    {
        int index = UnityEngine.Random.Range(0, spriteCount);
        string path = $"Items/Tier{tier}/Armors/{slot}/{type}/{rarity}/{slot.ToString().ToLower()}_{index}";
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
            Debug.LogWarning($"Sprite not found at path: {path}");

        return sprite;
    }

    private Sprite LoadRandomWeaponSprite(int tier, WeaponType type, EquipmentRarity rarity, int spriteCount)
    {
        int index = UnityEngine.Random.Range(0, spriteCount);
        string path = $"Items/Tier{tier}/Weapons/{type}/{rarity}/{type.ToString().ToLower()}_{index}";
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite == null)
            Debug.LogWarning($"Sprite not found at path: {path}");

        return sprite;
    }
}
