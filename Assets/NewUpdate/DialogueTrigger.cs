using System.Collections;
using System.Collections.Generic;
using Gamekit2D;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("Apply this field for 'Allies' characters.")]
    public GameObject character;
    public GameObject specialArea;
    public Dialogue dialogue;

    public UnityEvent OnDialoguePlay;
    public UnityEvent OnDialogueFinish;

    private DialogueManager dialogueManager;

    // Trigger the dialogue with optional events
    public void TriggerDialogue()
    {
        InvokeOnDialoguePlay();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

    }

    // Trigger the dialogue with automatic next sentence
    public void TriggerDialogueWithAutoNext(float delayTime)
    {
        StopAllCoroutines();

        InvokeOnDialoguePlay();
        FindObjectOfType<DialogueManager>().StartAutoNextSentence(dialogue, delayTime);
    }

    // Trigger the dialogue and load the scene
    public void TriggerDialogueAndLoadScene(string sceneName)
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
            dialogueManager.EndDialogueAndLoadScene(sceneName);
        }
        else
        {
            Debug.LogError("DialogueManager reference is null. Assign it in the Inspector.");
        }
    }

    // Enable or disable scripts of the character object
    public void DoEnableOrDisableScripts(bool type)
    {
        ToggleSpecialArea(type);
        ToggleCharacterScripts(type);
    }

    // Helper method to invoke OnDialogueFinish event after a delay
    private void InvokeOnDialoguePlay()
    {
        if (OnDialoguePlay != null)
        {
            float sentenceDisplayTime = 0.5f;
            float totalTime = dialogue.sentences.Length * sentenceDisplayTime;
            Debug.Log(totalTime);
            Invoke("FinishInvoke", totalTime);
        }
    }

    // Helper method to finish the dialogue
    private void PlayInvoke()
    {
        OnDialoguePlay.Invoke();
    }

    // Helper method to toggle the special area
    private void ToggleSpecialArea(bool type)
    {
        if (specialArea != null)
        {
            specialArea.SetActive(!type);
        }
    }

    // Helper method to toggle character scripts
    private void ToggleCharacterScripts(bool type)
    {
        if (character != null)
        {
            CharacterController2D characterController = character.GetComponent<CharacterController2D>();
            EnemyBehaviour enemyBehaviour = character.GetComponent<EnemyBehaviour>();

            if (characterController != null) characterController.enabled = type;
            if (enemyBehaviour != null) enemyBehaviour.enabled = type;
        }
    }
}
