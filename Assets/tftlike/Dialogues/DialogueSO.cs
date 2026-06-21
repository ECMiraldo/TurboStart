using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Dialogue"))]
public class DialogueSO : ScriptableObject
{
	[field: SerializeReference] public List<DialogueEffect> dialogueEffects = new();
	[field: SerializeField] public string dialogueName;
	[field: SerializeField] public TextAsset story;
	[field: SerializeField] public Sprite dialogueImage;

}
