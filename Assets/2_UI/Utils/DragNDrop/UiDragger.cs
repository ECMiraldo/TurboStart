using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class UiDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [field: Header("Dependencies")]
    [field: SerializeField] public UiSlotBase slot { get; private set; }
    public Transform parentAfterDrag { get; private set; }


    private Canvas rootCanvas;
    private RectTransform rectTransform;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;

    public bool isDragging { get; private set; }
    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentAfterDrag = transform.parent;

        if (UiManager.Instance == null) Debug.Log("Null canvas on " + transform.name + transform.parent.name);
        rootCanvas = UiManager.Instance.gameObject.GetComponent<Canvas>();
        if (rootCanvas == null) Debug.Log("Null canvas on " + transform.name);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (slot.item == null) return;
        isDragging = true;

        parentAfterDrag = transform.parent;
        originalOffsetMin = rectTransform.offsetMin;
        originalOffsetMax = rectTransform.offsetMax;
        originalAnchorMin = rectTransform.anchorMin;
        originalAnchorMax = rectTransform.anchorMax;
        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
        slot.itemImage.raycastTarget = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isDragging) transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        slot.itemImage.raycastTarget = true;
        transform.SetParent(parentAfterDrag.transform, false);
        rectTransform.anchorMin = originalAnchorMin;
        rectTransform.anchorMax = originalAnchorMax;
        rectTransform.offsetMin = originalOffsetMin;
        rectTransform.offsetMax = originalOffsetMax;
        transform.localScale = Vector3.one;
    }

}
