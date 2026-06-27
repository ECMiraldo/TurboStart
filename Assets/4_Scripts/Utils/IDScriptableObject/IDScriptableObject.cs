using UnityEngine;
using NaughtyAttributes;
public abstract class IDScriptableObject : ScriptableObject 
{
    [SerializeField] private string _id;
    public string id => _id;
}
