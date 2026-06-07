using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using NaughtyAttributes;

[RequireComponent(typeof(Image))]
public class UiDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> onBeginDrag;
    public event Action<PointerEventData> onEndDrag;

    [field: SerializeField, ReadOnly] public Transform parentObject { get; private set; }
    [field: SerializeField ] private Canvas rootCanvas;

    private Image icon;

    private Vector2 pivot;
    private RectTransform rectTransform;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;

    [field: SerializeField] public bool isDragging { get; private set; }
    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        parentObject = transform.parent;
        pivot = rectTransform.pivot;
        rootCanvas = GetComponentInParent<Canvas>()?.rootCanvas;
        if (rootCanvas == null) Debug.Log("Null canvas on " + transform.name);
        
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (icon == null) return;
        isDragging = true;
        parentObject = transform.parent;
        originalOffsetMin = rectTransform.offsetMin;
        originalOffsetMax = rectTransform.offsetMax;
        originalAnchorMin = rectTransform.anchorMin;
        originalAnchorMax = rectTransform.anchorMax;
        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
        onBeginDrag?.Invoke(eventData);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        rectTransform.localPosition = localPoint;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        icon.raycastTarget = true;
        isDragging = false;
        transform.SetParent(parentObject.transform, false);
        rectTransform.anchorMin = originalAnchorMin;
        rectTransform.anchorMax = originalAnchorMax;
        rectTransform.offsetMin = originalOffsetMin;
        rectTransform.offsetMax = originalOffsetMax;
        transform.localScale = Vector3.one;
        rectTransform.pivot = pivot;
        onEndDrag?.Invoke(eventData);
    }

}
