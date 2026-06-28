using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "City")]
public class CitySO : MapLocationSO
{
    public int shopSize = 5;

    public List<ShopEntry> possibleItems;

    public bool UpdateShop(CityData data, int currentDay)
    {
        int daysPassed = currentDay - (int)data.lastVisitedDay;

        // If no time passed, do nothing
        if (daysPassed <= 0 && data.shopItems != null && data.shopItems.Count > 0)
            return false;

        RefreshShop(data);
        data.lastVisitedDay = currentDay;
        return true;
    }

    private void RefreshShop(CityData data)
    {
        data.shopItems = new List<ShopEntry>();

        for (int i = 0; i < shopSize; i++)
        {
            ShopEntry randomItem = GetRandomItem();
            data.shopItems.Add(randomItem);
        }
    }

    private ShopEntry GetRandomItem()
    {
        if (possibleItems == null || possibleItems.Count == 0)
            return null;

        return possibleItems[Random.Range(0, possibleItems.Count)];
    }
}