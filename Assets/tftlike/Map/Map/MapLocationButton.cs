using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class MapLocationButton : MonoBehaviour
{
    [Header("Adventure Map Location")]
    [field: SerializeField] public virtual MapLocationSO mapLocationSO { get; }
    [field: SerializeField] protected MapLocationDetails stageDetails;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    protected virtual void Awake()
    {
        UpdateText();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowDetails);
    }

    protected virtual void UpdateText()
    {
        nameText.text = mapLocationSO.displayName;
    }

    protected virtual void ShowDetails() => stageDetails.ShowLocationDetails(this);
    public int GetRequiredTicks(MapLocationButton sourceLocation)
    {
        float dist = Vector2.Distance(transform.position, sourceLocation.transform.position);
        return Mathf.RoundToInt(dist * 10.0f);
    }

    public abstract void Enter();
}
