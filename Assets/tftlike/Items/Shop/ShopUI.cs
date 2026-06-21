using UnityEngine;
using Persistence;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotsPrefab;
    [SerializeField] private CityScreenUI cityScreen;
    private int nSlots = 0;

    private void OnEnable()
    {
        var shopData = cityScreen.currentCityData.shopItems;
        for (int i = 0; i < shopData.Count; i++)
        {
            
        }
    }
    private void TrimSlots()
    {

    }

}
