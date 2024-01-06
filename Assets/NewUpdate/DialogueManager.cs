using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject dialogueBox;

	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	private Queue<string> sentences;
	public int DialogueLength = 0;
	// private float delayTime = 1f;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}

	private void SetTextCharacterTextName(String characterName) {
		if (dialogueText == null || nameText == null)
		{
			Debug.LogError("dialogueText or nameText is not assigned in the Unity Editor.");
			return;
		}

		nameText.text = characterName;
	}
	private void SetColorCharacterTextName(String characterName) {
		if(characterName == "Chupter") {nameText.color = Color.magenta;}
		else if (characterName == "Giọng nói thiên thần") {nameText.color = Color.blue;}
		else if (characterName == "Hệ thống") {nameText.color = Color.black;}
		else {nameText.color = Color.cyan;}
	}
	

	public void StartDialogue (Dialogue dialogue)
	{
		// PlayInvoke();

		animator.SetBool("IsOpen", true);

		SetTextCharacterTextName(dialogue.name);

		SetColorCharacterTextName(dialogue.name);

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();

		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		if(DialogueLength > 0) DialogueLength = DialogueLength - 1;
		else if (DialogueLength <= 0) DialogueLength = 0;

		Debug.Log(DialogueLength);

		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void StartAutoNextSentence(Dialogue dialogue, float delayTime)
	{
		StartCoroutine(AutoNextSentence(dialogue, delayTime));
	}

	private IEnumerator AutoNextSentence(Dialogue dialogue, float delayTime)
	{
		// PlayInvoke();

		animator.SetBool("IsOpen", true);

		SetTextCharacterTextName(dialogue.name);

		SetColorCharacterTextName(dialogue.name);

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
			
			if (sentences.Count == 0)
			{
				EndDialogue();
				break;
			}

			string talk = sentences.Dequeue();
			StartCoroutine(TypeSentence(talk));
			
			Debug.Log(dialogue.sentences.Length);
			yield return new WaitForSeconds(delayTime);
		}
		Debug.Log(dialogue.sentences.Length);
	}

	public void EndDialogueAndLoadScene(string sceneName)
	{
		EndDialogue();
		StartCoroutine(LoadSceneAfterDialogue(sceneName));
	}

	private IEnumerator LoadSceneAfterDialogue(string sceneName)
	{
		yield return new WaitForSeconds(1f); // Đợi 1 giây trước khi chuyển scene (hoặc thay đổi thời gian mong muốn)
		// Load scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		DialogueLength = 0;
	}

}
