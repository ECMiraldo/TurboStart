using System;

public enum PartyStat : byte
{
    DropChance = 0,
    GoldDrop = 1,
    ExperienceGain = 2,
}

public enum UnitStat : byte
{
    Attack = 0,
    MagicAttack = 1,
    AttackSpeed = 2,
    AttackRange = 3,
    Accuracy = 4,
    CritChance = 5,
    Health = 6,
    Focus = 7,
    Defense = 8,
    MagicDefense = 9,
    Evasion = 10,
    Lifesteal = 11,
    FocusPerHit = 12,
    FocusOnDamageTaken = 13,
    FocusPerSecond = 14,
    DamageReduction = 15,
}
