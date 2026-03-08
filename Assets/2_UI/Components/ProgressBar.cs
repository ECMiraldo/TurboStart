using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    public event Action onComplete;

    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private float duration = 2f;

    

    private Slider slider;
    private Coroutine fillRoutine;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            StartFill(duration);
        }
    }

    private void OnDisable()
    {
        StopFill();
    }

    public void StartFill(float duration)
    {
        StopFill();
        fillRoutine = StartCoroutine(FillRoutine(duration));
    }

    public void StopFill()
    {
        if (fillRoutine != null)
        {
            StopCoroutine(fillRoutine);
            fillRoutine = null;
        }
    }

    private IEnumerator FillRoutine(float duration)
    {
        slider.value = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = time / duration;
            yield return null;
        }

        slider.value = 1f;
        fillRoutine = null;
        onComplete?.Invoke();
    }
}