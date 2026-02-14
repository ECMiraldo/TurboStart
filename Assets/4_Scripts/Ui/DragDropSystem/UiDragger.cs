using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


[RequireComponent(typeof(Image))]
public class UiDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> onBeginDrag;
    public event Action<PointerEventData> onEndDrag;

    private Transform parentAfterDrag;
    private Canvas rootCanvas;
    private Image icon;

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
        parentAfterDrag = transform.parent;
        rootCanvas = GameObject.FindGameObjectWithTag("RootCanvas").GetComponent<Canvas>();
        if (rootCanvas == null) Debug.Log("Null canvas on " + transform.name);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (icon == null) return;
        isDragging = true;
        parentAfterDrag = transform.parent;
        originalOffsetMin = rectTransform.offsetMin;
        originalOffsetMax = rectTransform.offsetMax;
        originalAnchorMin = rectTransform.anchorMin;
        originalAnchorMax = rectTransform.anchorMax;
        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
        onBeginDrag?.Invoke(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isDragging) transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        icon.raycastTarget = true;
        transform.SetParent(parentAfterDrag.transform, false);
        rectTransform.anchorMin = originalAnchorMin;
        rectTransform.anchorMax = originalAnchorMax;
        rectTransform.offsetMin = originalOffsetMin;
        rectTransform.offsetMax = originalOffsetMax;
        transform.localScale = Vector3.one;
        onEndDrag?.Invoke(eventData);
    }

}
