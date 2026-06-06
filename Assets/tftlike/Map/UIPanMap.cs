using UnityEngine;


using UnityEngine.EventSystems;

public class UIPanMap :MonoBehaviour, IBeginDragHandler,IDragHandler
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta;
    }
}
