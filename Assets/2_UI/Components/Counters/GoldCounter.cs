using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    private void OnEnable()
    {
        ResourceData.onGoldChanged += OnGoldChanged;
        OnGoldChanged(Persistence.SaveLoadSystem.Instance.data.resourceData.gold);
    }

    private void OnDisable()
    {
        ResourceData.onGoldChanged -= OnGoldChanged;
    }

    private void OnGoldChanged(long amount)
    {
        amountText.text = amount.ToString();
    }
}
