using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(Image))]
public class ToggleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform handle;
    [SerializeField] private Image backgroundImage;

    [Header("Settings")]
    [SerializeField] private float handleOffset = 2f;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private Toggle toggle;
    private RectTransform rectTransform;

    private Coroutine moveRoutine;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        rectTransform = GetComponent<RectTransform>();
        backgroundImage = GetComponent<Image>();
        toggle.onValueChanged.AddListener(OnToggleChanged);

        // Snap to initial state
        SetImmediate(toggle.isOn);
    }

    private void OnToggleChanged(bool isOn)
    {
        // Stop any running animation
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveHandle(isOn));
    }

    private IEnumerator MoveHandle(bool isOn)
    {
        float startX = handle.localPosition.x;

        float onPosX = (rectTransform.sizeDelta.x / 2f) - (handle.sizeDelta.x / 2f) - handleOffset;
        float offPosX = -onPosX;

        float targetX = isOn ? onPosX : offPosX;

        Color startColor = backgroundImage.color;
        Color targetColor = isOn ? onColor : offColor;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            float newX = Mathf.Lerp(startX, targetX, t);
            handle.localPosition = new Vector3(newX, 0f, 0f);

            backgroundImage.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        backgroundImage.color = targetColor;

        // Snap exactly to target (prevents tiny precision drift)
        handle.localPosition = new Vector3(targetX, 0f, 0f);

        moveRoutine = null;
    }

    private void SetImmediate(bool isOn)
    {
        float onPosX = (rectTransform.sizeDelta.x / 2f) - (handle.sizeDelta.x / 2f) - handleOffset;
        float offPosX = -onPosX;

        float targetX = isOn ? onPosX : offPosX;
        handle.localPosition = new Vector3(targetX, 0f, 0f);
        backgroundImage.color = isOn ? onColor : offColor;
    }
}