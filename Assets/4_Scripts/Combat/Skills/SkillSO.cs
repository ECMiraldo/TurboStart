using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Skill")]
public class SkillSO : IDScriptableObject
{
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public GameObject prefab { get; private set; }
    [field: SerializeField] public int minFocusCost { get; private set; }
    [field: SerializeField] public DamageType damageType { get; private set; }

    [field: SerializeReference] public List<CombatEffect> onCastEffects = new();
    [field: SerializeReference] public List<CombatEffect> onDamageEffects = new();




    
}
