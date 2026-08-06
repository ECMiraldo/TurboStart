using UnityEngine;
using Persistence;
using System.Collections;

public class GameGuide1 : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;

    IEnumerator Start()
    {
        if (SaveLoadSystem.Instance.data.progressionData.hasSeenGameGuide1)
        {
            Destroy(this.gameObject);
            yield return null;
        }
        yield return new WaitForSeconds(1f); // Wait for 1 second before starting the dialogue
        DialogueHandler.Instance.StartDialogue(dialogue);
    }
}
