using UnityEngine;
using System;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public static event Action<StatsComponent> OnUnitDied;
    public event Action OnDeath;
    

    [SerializeField] private Slider bar;


    private StatsComponent stats;
    private Attribute healthStat;

    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public void Init(StatsComponent stats)
    {
        this.stats = stats;
        healthStat = stats.stats[UnitStat.Health];
        CurrentHealth = healthStat.ToInt();
        healthStat.OnValueChanged += UpdateMaxHealth;
        UpdateMaxHealth(healthStat.ToInt());
    }

    private void OnDisable()
    {
        healthStat.OnValueChanged -= UpdateMaxHealth;
    }

    private void UpdateMaxHealth(float newValue)
    {
        CurrentHealth = Mathf.Min(CurrentHealth, healthStat.ToInt());
        UpdateUi();
    }


    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        amount = Mathf.Max(amount, 0);
        CurrentHealth -= amount;


        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
            OnUnitDied?.Invoke(stats);
        }
        UpdateUi();
    }

    private void UpdateUi()
    {
        bar.enabled = !IsDead;
        bar.maxValue = healthStat.ToInt();
        bar.value = CurrentHealth;
    }
}
