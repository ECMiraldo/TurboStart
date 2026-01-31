using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour, IPooledObject
{
    [field:SerializeField] public ObjectPoolSettings poolSettings { get; private set; }
    [SerializeField] private TextMeshProUGUI textmeshPro;
    [SerializeField] private Image possibleImage;

    public void ShowNumber(string text, float timeShown = 1.5f )
    {
        textmeshPro.text = text;
    }
}
