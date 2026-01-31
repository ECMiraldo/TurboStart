using UnityEngine;
using System;

public class GridHealthComponent : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10;
    public int CurrentHealth { get; private set; }


    public bool IsDead => CurrentHealth <= 0;


    public event System.Action OnDeath;


    private void Awake()
    {
        CurrentHealth = maxHealth;
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
    }
}
