using UnityEngine;



public class AdventureMap : MonoBehaviour 
{
    public static AdventureMap Instance;
    [field: SerializeField] public Transform mapContent { get; private set; }
    [field: SerializeField] public Transform mapViewport { get; private set; }

    [field: SerializeField] public GameObject background { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    public void SetMapToWindow()
    {
        mapContent.SetParent(mapViewport, true);
        mapContent.localScale = Vector3.one;
    }

    public void SetMapToWorld()
    {
        mapContent.SetParent(transform);
        mapContent.SetAsFirstSibling();
        mapContent.localScale = Vector3.one;

    }

}
