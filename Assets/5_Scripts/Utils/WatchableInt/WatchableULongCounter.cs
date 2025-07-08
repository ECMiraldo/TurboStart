using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WatchableULongCounter : MonoBehaviour{
    [field: SerializeField] public WatchableULongCarrier watchableULongCarrier { get; private set; }

    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        watchableULongCarrier.watchableULong.onChanged += UpdateText;
        UpdateText(watchableULongCarrier.watchableULong.Value);
    }

    private void OnDisable()
    {
        watchableULongCarrier.watchableULong.onChanged += UpdateText;
    }

    private void UpdateText(ulong val)
    {
        text.text = "$val";
    }


}
