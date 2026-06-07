using UnityEngine;



public class AdventureMap : MonoBehaviour 
{
    public static AdventureMap Instance;

    [field: SerializeField] public UIPanMap mapView { get; private set; }
    [field: SerializeField] public Transform mapViewport { get; private set; }
    [field: SerializeField] public GameObject background { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    public void SetMapToWindow()
    {
        mapView.transform.SetParent(mapViewport, false); ;
        mapView.transform.localScale = Vector3.one;
        mapView.SetViewport(mapViewport.GetComponent<RectTransform>());
    }

    public void SetMapToWorld()
    {
        mapView.transform.SetParent(transform, false);
        mapView.transform.SetAsFirstSibling();
        mapView.transform.localScale = Vector3.one;
        mapView.SetViewport(transform.GetComponent<RectTransform>());

    }

}
