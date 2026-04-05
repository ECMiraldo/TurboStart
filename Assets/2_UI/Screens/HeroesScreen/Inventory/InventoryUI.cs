using UnityEngine;
using Persistence;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotsPrefab;

    private Inventory inventory;
    private int nSlots = 0;

    private void Awake()
    {
        inventory = SaveLoadSystem.Instance.data.inventory;

    }
    private void OnEnable()
    {
        for (int i = nSlots; i < inventory.maxItems; i++)
        {
            InstantiateSlot();
        }
        nSlots = inventory.maxItems;
    }

    private void InstantiateSlot()
    {
        Instantiate(slotsPrefab, slotsParent);
    }

}
