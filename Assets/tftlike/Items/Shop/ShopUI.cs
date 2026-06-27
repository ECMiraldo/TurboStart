using UnityEngine;
using Persistence;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotsPrefab;
    [SerializeField] private CityScreenUI cityScreen;
    private int nSlots = 0;
    private List<ShopSlot> slots;

    private void Awake()
    {
        slots = new();
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots.Add(slotsParent.GetChild(i).GetComponent<ShopSlot>());
        }
    }

    private void OnEnable()
    {
        int currentDay = TickManager.Instance.CurrentDay;
        if (cityScreen.currentCity.UpdateShop(cityScreen.currentCityData, currentDay))
        {
            var shopData = cityScreen.currentCityData.shopItems;
            for (int i = 0; i < shopData.Count; i++)
            {
                slots[i].SetItem(shopData[i].item, shopData[i].quantity);
            }
            TrimSlots(shopData.Count);
        }
    }
    private void TrimSlots(int startIndex)
    {
        for (int i = startIndex; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

}
