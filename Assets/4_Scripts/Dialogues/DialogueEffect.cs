using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public abstract class DialogueEffect
{
	[SerializeField] public string tagID;
	public abstract IEnumerator Execute();
}

[Serializable]
public class AddItemEffect : DialogueEffect
{
	public override IEnumerator Execute()
	{
		Debug.Log("item added ");
		yield return null;
	}
}


[Serializable]
public class GiveGoldEffect : DialogueEffect
{
	[SerializeField] public int goldAmount;

	public override IEnumerator Execute()
	{
		Debug.Log("Gold given " + goldAmount);
		yield return null;
	}
}

[Serializable]
public class StartStageEffect : DialogueEffect
{
	[SerializeField] public StageDefinitionSO stage;

	public override IEnumerator Execute()
	{
		CombatSessionManager.Instance.StartStage(stage);
		yield return null;
	}
}