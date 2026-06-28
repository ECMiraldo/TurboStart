using Persistence;
using UnityEngine;

public class UIHeroBoard : MonoBehaviour
{
    [SerializeField] private Transform listContent;
    [SerializeField] private GameObject heroCardPrefab;

    // from ui button
    public void StartRound()
    {
        CombatSessionManager.Instance.BeginNextRound();
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        CreateHeroCards();
        
    }

    void OnDisable()
    {
        ClearHeroCards();
    }



    private void CreateHeroCards()
    {
        foreach (HeroData data in SaveLoadSystem.Instance.data.heroes)
        {
            UIHeroIcon icon = Instantiate(heroCardPrefab, listContent).GetComponent<UIHeroIcon>();
            icon.SetData(data);
        }
    }

    private void ClearHeroCards()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(0).gameObject);
        }
    }
}
