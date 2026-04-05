using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : UiSlot<Equipment>
{
    [field: SerializeField] public UiDragger dragger { get; private set; }
    [field: SerializeField] public Image icon { get; private set; }
    [field: SerializeField] public EquipmentSlot slot { get; private set; }


    public override void OnDrop(PointerEventData eventData)
    {
        var parentSlot = eventData.pointerDrag.GetComponent<UiDragger>().parentObject.GetComponent<UiSlot<Item>>();
        print(parentSlot.currentItem);
    }


}

