using UnityEngine;
using System;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public event Action OnDeath;


    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Slider bar;
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    public void Init(StatsComponent stats)
    {
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
