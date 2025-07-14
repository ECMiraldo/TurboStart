using System;
using UnityEngine;

[Serializable]
public class FighterData
{

    [field: Header("Physical")]
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField] public int Accuracy { get; private set; }
    [field: SerializeField] public float CritChance { get; private set; }


    [field: Header("Magic")]
    [field: SerializeField] public int MagicAttack { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public float ManaRegen { get; private set; }


    [field: Header("Defense")]
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float HealthRegen { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int MagicDefense { get; private set; }
    [field: SerializeField] public int Evasion { get; private set; }


    [field: Header("Movement")]
    [field: SerializeField] public float MoveSpeed { get; private set; }

}


