using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;






public abstract class UiSlotBase : MonoBehaviour
{
    [field: SerializeField] public Image itemImage { get; protected set; }
    [field: SerializeField] public TextMeshProUGUI slotText { get; protected set; }
    public abstract object item { get; }
}

public abstract class UiSlot<T> : UiSlotBase, IDropHandler, IPointerClickHandler where T : class, IDraggable
{
    [field: SerializeReference] public T currentItem { get; protected set; }
    public override object item => currentItem;
    public virtual void SetItem(T item, string text) 
    {
        currentItem = item;
        slotText.text = text;
        itemImage.sprite = item.Sprite;
        itemImage.enabled = true;
    }
    public virtual void RemoveItem()
    {
        slotText.text = string.Empty;
        currentItem = null;
        itemImage.enabled = false;
    }

    public abstract void OnDrop(PointerEventData eventData);

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            OnShiftClicked();
        }
        else if (eventData.clickCount == 2)
        {
            OnDoubleClicked();
        }
        else OnNormalClick();
    }

    #region ClickModifiers
    protected virtual void OnShiftClicked()
    {


    }

    protected virtual void OnDoubleClicked()
    {
    }

    protected virtual void OnNormalClick()
    {


    }
    protected virtual void OnRightClick()
    {
    }

    #endregion


}
