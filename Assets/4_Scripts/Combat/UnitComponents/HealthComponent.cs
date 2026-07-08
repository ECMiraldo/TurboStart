using UnityEngine;
using System;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public event Action OnDeath;
    public static event Action<StatsComponent> OnUnitDied;

    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Slider bar;
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;
    private StatsComponent stats;
    public void Init(StatsComponent stats)
    {
        this.stats = stats;
        maxHealth = stats.stats[UnitStat.Health].ToInt();
        CurrentHealth = maxHealth;
        UpdateUi();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;


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
        bar.maxValue = maxHealth;
        bar.value = CurrentHealth;
    }
}
