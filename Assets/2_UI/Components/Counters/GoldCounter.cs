using TMPro;
using UnityEngine;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    private void OnEnable()
    {
        PersistentData.onGoldChanged += OnGoldChanged;
        OnGoldChanged(Persistence.SaveLoadSystem.Instance.data.gold);
    }

    private void OnDisable()
    {
        PersistentData.onGoldChanged -= OnGoldChanged;
    }

    private void OnGoldChanged(long amount)
    {
        amountText.text = amount.ToString();
    }
}
