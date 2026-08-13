

using UnityEngine;

public enum DamageType
{
    Physical,
    Magical,
    True
}

public struct Damage
{
    public static void Create(UnitContext source, UnitContext target, DamageType type, int amount)
    {
        float finalDamage;
        if (type == DamageType.True) finalDamage = amount;
        else
        {
            float reduction = type == DamageType.Physical
                ? target.Stats.stats[UnitStat.Defense].Value
                : target.Stats.stats[UnitStat.MagicDefense].Value;

            float mitigation = Mathf.Min(
                0.75f,
                Mathf.Pow(reduction, 0.5f) / 100f
            );

            finalDamage = amount * (1f - mitigation);
        }

        int finalDamageInt = Mathf.Max(
            0,
            Mathf.FloorToInt(finalDamage)
        );

        target.Health.TakeDamage(finalDamageInt);

        DamageNumberManager.ShowNumber(
            target.Grid.transform.position,
            finalDamageInt.ToString()
        );
    }

}
