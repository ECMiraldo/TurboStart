using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class FocusComponent : MonoBehaviour
{
    [SerializeField] private Slider bar;

    private SkillSO skill;
    private StatsComponent stats;

    private Attribute focusStat;
    private Attribute focusPerHitStat;
    private Attribute focusPerDamageTakenStat;
    private Attribute focusPerSecondStat;

    // Float internally so focusPerSecond can accumulate fractional values.
    private float currentFocus;

    public int CurrentFocus => Mathf.FloorToInt(currentFocus);

    public void Init(StatsComponent stats)
    {
        this.stats = stats;

        focusStat = stats.stats[UnitStat.Focus];
        focusPerHitStat = stats.stats[UnitStat.FocusPerHit];
        focusPerDamageTakenStat = stats.stats[UnitStat.FocusOnDamageTaken];
        focusPerSecondStat = stats.stats[UnitStat.FocusPerSecond];

        skill = stats.unitData.skill;

        currentFocus = 0f;

        focusStat.OnValueChanged += UpdateMaxFocus;
        stats.ctx.Health.OnDamageTaken += OnDamageTaken;
        stats.ctx.Visuals.onAttack += OnAttack;

        UpdateMaxFocus(focusStat.Value);
    }

    private void Update() => GainFocus(focusPerSecondStat.Value * Time.deltaTime);
    private void OnAttack(UnitContext ctx) => GainFocus(focusPerHitStat.Value);
    private void OnDamageTaken(float damage) => GainFocus(focusPerDamageTakenStat.Value);

    private void OnDisable()
    {
        focusStat.OnValueChanged -= UpdateMaxFocus;
        stats.ctx.Health.OnDamageTaken -= OnDamageTaken;
        stats.ctx.Visuals.onAttack -= OnAttack;
    }

    private void UpdateMaxFocus(float newValue)
    {
        currentFocus = Mathf.Min(currentFocus, focusStat.ToInt());

        UpdateUI();
    }


    public void GainFocus(float amount)
    {
        if (amount <= 0f)
            return;

        currentFocus += amount;

        if (currentFocus >= focusStat.ToInt())
        {
            currentFocus = 0f;
            if (skill != null) 
                StartCoroutine(skill.Cast(stats.ctx));
        }

        UpdateUI();
    }

    public void ConsumeAll()
    {
        currentFocus = 0f;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (bar == null || focusStat == null)
            return;

        bar.maxValue = focusStat.ToInt();
        bar.value = currentFocus;
    }
}