using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Projectile", menuName = "Combat/Projectile")]
public class ProjectileSO : ScriptableObject
{
    [field: SerializeField] public GameObject projectilePrefab { get; private set; }
    [field: SerializeField] public float speed { get; private set; }
    
    [SerializeReference] public List<CombatEffect> onDamageEffects = new();

}

