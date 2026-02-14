using Persistence;
using UnityEngine;

public class UIHeroBoard : MonoBehaviour
{
    [SerializeField] private Transform listContent;
    [SerializeField] private GameObject heroCardPrefab;

    private void Start()
    {
        gameObject.SetActive(false);
        CombatSessionManager.onStateChanged += OnStateChanged;
    }

    // from ui button
    public void StartRound()
    {
        CombatSessionManager.Instance.BeginNextRound();
    }

    private void OnStateChanged(CombatState state)
    {
        if (state == CombatState.StageSetup) gameObject.SetActive(true);
        if (state == CombatState.Spawning) gameObject.SetActive(false);
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
