using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : UiSlot<Item>
{
    [field: SerializeField] private TextMeshProUGUI quantityText;
    [field: SerializeField] public ShopUI shopUI;


    public void SetItem(Item item, int qnt)
    {
        quantityText.text = qnt.ToString();
        base.SetItem(item);
    }
    
    public override void OnDrop(PointerEventData eventData)
    {
        var parentSlot = eventData.pointerDrag.GetComponent<UiDragger>().parentObject.GetComponent<InventorySlot>();
        if (!parentSlot) return;
        Item item = parentSlot.currentItem;
        if (item != null)
        {
            parentSlot.SetItem(null);
            SaveLoadSystem.Instance.data.inventory.RemoveEntry(parentSlot.entry.index);
        }
    }
}
