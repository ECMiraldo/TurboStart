

public enum DamageType
{
    Physical,
    Magical,
    True
}

public struct Damage
{
    DamageType type;
    int amount;

    public static void Create(UnitContext source, UnitContext target, DamageType type, int amount)
    {
        int finalDamage = type == DamageType.Physical && type != DamageType.True ?
            amount - target.Stats.stats[UnitStat.Defense].ToInt() : 
            amount - target.Stats.stats[UnitStat.MagicDefense].ToInt();


        target.Health.TakeDamage(finalDamage);
        DamageNumberManager.ShowNumber(target.Grid.transform.position, finalDamage.ToString());

    }


}