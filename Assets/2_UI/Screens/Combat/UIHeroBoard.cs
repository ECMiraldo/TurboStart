using Persistence;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroBoard : MonoBehaviour
{
    [SerializeField] private Transform listContent;
    [SerializeField] private GameObject heroCardPrefab;
    [SerializeField] private Button startRoundButton;

    // from ui button
    public void StartRound()
    {
        CombatSessionManager.Instance.BeginRound(0);
        startRoundButton.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        CreateHeroCards();
        startRoundButton.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        ClearHeroCards();
    }

    private void CreateHeroCards()
    {
        foreach (HeroData data in SaveLoadSystem.Instance.data.partyData.heroes)
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
