using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class UnitVitals
{
    public event Action<int> OnHealthChanged;
    public event Action<int> OnManaChanged;
    public event Action OnDied;

    [JsonProperty] [field: SerializeField] public int currentHealth { get; private set; }
    [JsonProperty] [field: SerializeField] public int currentMana { get; private set; }

    [JsonIgnore] private UnitStats unitStats;

    public void FullRegen()
    {
        if (unitStats == null) return;
        IncrementHealth(unitStats.stats[UnitStat.Health].ToInt());
        IncrementMana(unitStats.stats[UnitStat.Mana].ToInt());
    }

    public void InjectStats(UnitStats stats)
    {
        this.unitStats = stats;
        //handles regen in case the vitals are being created and not loaded.
        if (currentHealth == 0 && currentMana == 0)
        {
            IncrementHealth(unitStats.stats[UnitStat.Health].ToInt());
            IncrementMana(unitStats.stats[UnitStat.Mana].ToInt());
        }
    }
    public void IncrementHealth(int health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, unitStats.stats[UnitStat.Health].ToInt());
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth == 0)
        {
            //TODO: handle die
            OnDied?.Invoke();
        }
    }

    public void IncrementMana(int mana)
    {
        currentMana = Mathf.Clamp(currentMana + mana, 0, unitStats.stats[UnitStat.Mana].ToInt());
        OnManaChanged?.Invoke(currentMana);
    }


}



