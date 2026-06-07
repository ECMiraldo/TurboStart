using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanMap : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] public RectTransform viewport;

    private RectTransform rectTransform;
    private Canvas canvas;

    private readonly Vector3[] mapCorners = new Vector3[4];
    private readonly Vector3[] viewportCorners = new Vector3[4];

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SetViewport(transform.parent.GetComponent<RectTransform>());
    }

    public void SetViewport(RectTransform transform)
    {
        viewport = transform;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position - eventData.delta,
            eventData.pressEventCamera,
            out Vector2 prevLocal
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 currentLocal
        );

        Vector2 delta = currentLocal - prevLocal;

        rectTransform.anchoredPosition += delta;

        ClampToViewport();
    }

    private void ClampToViewport()
    {
        rectTransform.GetWorldCorners(mapCorners);
        viewport.GetWorldCorners(viewportCorners);

        Vector2 delta = Vector2.zero;

        // Left
        if (mapCorners[0].x > viewportCorners[0].x)
            delta.x = viewportCorners[0].x - mapCorners[0].x;

        // Right
        if (mapCorners[2].x < viewportCorners[2].x)
            delta.x = viewportCorners[2].x - mapCorners[2].x;

        // Bottom
        if (mapCorners[0].y > viewportCorners[0].y)
            delta.y = viewportCorners[0].y - mapCorners[0].y;

        // Top
        if (mapCorners[2].y < viewportCorners[2].y)
            delta.y = viewportCorners[2].y - mapCorners[2].y;

        rectTransform.anchoredPosition += delta;
    }


}