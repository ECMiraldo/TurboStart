using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageNumber : MonoBehaviour, IPooledObject
{
    [field:SerializeField] public ObjectPoolSettings poolSettings { get; private set; }
    [SerializeField] private TextMeshProUGUI textmeshPro;
    [SerializeField] private Image possibleImage;

    [SerializeField] private float yIncrease = 2;
    [SerializeField] private float scaleMultiply = 0.5f;
    [SerializeField] protected float animationDuration = 0.1f;

    public void ShowNumber(string text, float timeShown = 1.5f )
    {
        DOTween.Kill(transform);

        textmeshPro.text = text;

        RectTransform rt = (RectTransform)transform;
        rt.localScale = Vector3.one;

        rt.DOAnchorPosY(rt.anchoredPosition.y + yIncrease, animationDuration)
              .SetEase(Ease.OutQuad);

        rt.DOScale(scaleMultiply, animationDuration)
              .SetEase(Ease.OutQuad);
    }
}
