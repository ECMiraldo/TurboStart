using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHeroIcon : MonoBehaviour
{
    [field: SerializeField] public Image icon { get; private set; }
    [field: SerializeField] public UiDragger dagger { get; private set; }


    [field: SerializeField] public HeroData heroData { get; private set; }



    private void OnEnable()
    {
        dagger.onBeginDrag += OnBeginDrag;
        dagger.onEndDrag += OnEndDrag;
    }
    private void OnDisable()
    {
        dagger.onBeginDrag -= OnBeginDrag;
        dagger.onEndDrag -= OnEndDrag;
    }

    private void OnBeginDrag(PointerEventData data)
    {
        print("drag begin");
    }


    private void OnEndDrag(PointerEventData data)
    {
        print("drag ended");
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(data.position);
        if (CombatSessionManager.Instance.spawner.PlaceHero(heroData, worldPos))
        {
            Destroy(this.gameObject);
        }
    }

    public void SetData(HeroData data)
    {
        heroData = data;
        icon.sprite = data.template.icon;


    }


}
