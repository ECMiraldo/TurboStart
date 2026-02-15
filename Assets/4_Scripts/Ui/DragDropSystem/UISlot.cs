using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UiSlotBase : MonoBehaviour
{
    [field: SerializeField] public Image itemImage { get; protected set; }
    public abstract object item { get; }
}


public abstract class UiSlot<T> : UiSlotBase, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler where T : class, IDraggable
{
    [field: SerializeField] public T currentItem { get; protected set; }
    public override object item => currentItem;
    public virtual void SetItem(T item) 
    {
        if (item == null )
        {
            RemoveItem();
            return;
        }
        currentItem = item;
        itemImage.sprite = item.Sprite;
        itemImage.enabled = true;
    }
    public virtual void RemoveItem()
    {
        currentItem = null;
        itemImage.enabled = false;
    }

    public abstract void OnDrop(PointerEventData eventData);

    #region PointerEnterExit
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    #endregion

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
