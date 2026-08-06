using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[Serializable]
public class ChoiceButtons
{
	[SerializeField] public TextMeshProUGUI choiceText;
	[SerializeField] public Button button;
}

public class DialogueHandler : UIPanelController
{
	public static DialogueHandler Instance { get; private set; }

	[Header("References")]
	[SerializeField] private Image dialogueImage;
	[SerializeField] private List<ChoiceButtons> choiceButtons;
	[SerializeField] private TextMeshProUGUI mainText;
	[SerializeField] private TextMeshProUGUI speakerTitle;

	[Header("Config")]
	[SerializeField] private float typewriterTimeInterval;
	[SerializeField] private bool isTypewriterRunning;
	private Coroutine typewriterCoroutine;

	[Header("Current Story")]
   	[SerializeField] private DialogueSO dialogue;
	private Story story;

	protected override void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	protected override void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			int iCopy = i;
			choiceButtons[i].button.onClick.AddListener(() => OnChoiceClicked(iCopy));
		}
		base.Start();
	}

	public void StartDialogue(DialogueSO dialogue)
	{
		HideAllChoices();
		Open();
		this.dialogue = dialogue;
		speakerTitle.text = dialogue.name;
		if (dialogue.dialogueImage != null) dialogueImage.sprite = dialogue.dialogueImage;
		story = new Story(dialogue.story.text);
		ContinueStory();
	}

	private void EndDialogue()
	{
		story = null;
		dialogue = null;
		HideAllChoices();
		Close();
	}

	private void Update()
	{
		if (story == null) return;
		
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
		{
			if (isTypewriterRunning  && typewriterCoroutine != null) Halt();
			else if (story.canContinue) ContinueStory(); // If there's more story, continue
			else if (story.currentChoices.Count > 0) ShowChoices();
			else EndDialogue(); // If no more story and no choices, end the dialogue
		}
	}

	private void ContinueStory()
	{
		 HideAllChoices();

		string text = story.Continue();

		if (!string.IsNullOrWhiteSpace(text))
		{
			typewriterCoroutine = StartCoroutine(IncreaseMaxVisibleChar(text));
		}

		HandleStoryTags(story.currentTags);

		if (!story.canContinue &&
			story.currentChoices.Count == 0 &&
			string.IsNullOrWhiteSpace(text))
		{
			EndDialogue();
		}

	}

	public void OnChoiceClicked(int index)
	{
		// Hide choices immediately upon selection
		HideAllChoices();
		story.ChooseChoiceIndex(index);
		if (story.currentTags.Count > 0) HandleStoryTags(story.currentTags);
		if (story.canContinue) ContinueStory();
		else EndDialogue();
	}


	private void ShowChoices()
	{
		int i;
		for (i = 0; i < story.currentChoices.Count; i++)
		{
			Choice choice = story.currentChoices[i];
			choiceButtons[i].button.gameObject.SetActive(true);
			choiceButtons[i].choiceText.text = choice.text;
		}

		// Hide any unused choice buttons
		for (int j = i; j < 4; j++)
		{
			choiceButtons[j].button.gameObject.SetActive(false);
		}
	}

	private void HideAllChoices()
	{
		for (int i = 0; i < 4; i++)
		{
			choiceButtons[i].button.gameObject.SetActive(false);
		}
	}


	private void HandleStoryTags(List<string> tags)
	{
		foreach (string tag in tags)
		{
			if (tag.StartsWith("Trigger:"))
			{
				string effectID = tag.Split(":")[1];
				var allEffects = dialogue.dialogueEffects.FindAll((x) => x.tagID == effectID);
				foreach (var efx in allEffects) StartCoroutine(efx.Execute());
			}
			if (tag.StartsWith("Speaker:"))
			{
				string speakerName = tag.Split(":")[1];
				speakerTitle.text = speakerName;
			}
		}
	}

	
	IEnumerator IncreaseMaxVisibleChar(string message)
    {
		mainText.text = message;
        mainText.maxVisibleCharacters = 0;
        int messageCharLength = message.Length;
        while (mainText.maxVisibleCharacters < messageCharLength)
        {
            mainText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typewriterTimeInterval);
        }

        isTypewriterRunning = false;
    }

	    //Let player see the whole message in instant
    public void Halt()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }

        //mainText.text = m_Message;
        mainText.maxVisibleCharacters = int.MaxValue;
        print("Typewriter effect halted by player");
        isTypewriterRunning = false;
    }

}
