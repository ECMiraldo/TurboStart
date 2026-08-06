using UnityEngine;
using UnityEngine.UI;
using System;

public class FocusComponent : MonoBehaviour
{
    public event Action OnFocusFull;

    [SerializeField] private Slider bar;

    private StatsComponent stats;
    private Attribute focusStat;

    public int CurrentFocus { get; private set; }

    public void Init(StatsComponent stats)
    {
        this.stats = stats;

        focusStat = stats.stats[UnitStat.Focus];

        CurrentFocus = 0;

        focusStat.OnValueChanged += UpdateMaxFocus;

        UpdateMaxFocus(focusStat.Value);
    }

    private void OnDisable()
    {
        if (focusStat != null)
            focusStat.OnValueChanged -= UpdateMaxFocus;
    }

    private void UpdateMaxFocus(float newValue)
    {
        CurrentFocus = Mathf.Min(CurrentFocus, focusStat.ToInt());
        UpdateUI();
    }

    public void GainFocus(int amount)
    {
        if (amount <= 0)
            return;

        CurrentFocus += amount;

        if (CurrentFocus >= focusStat.ToInt())
        {
            CurrentFocus = focusStat.ToInt();

            UpdateUI();

            OnFocusFull?.Invoke();
        }
        else
        {
            UpdateUI();
        }
    }

    public void ConsumeAll()
    {
        CurrentFocus = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        bar.maxValue = focusStat.ToInt();
        bar.value = CurrentFocus;
    }
}