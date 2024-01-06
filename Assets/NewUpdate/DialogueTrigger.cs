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

    void Start() {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void PlayInvoke() {
        Debug.Log(dialogueManager.DialogueLength);
        if(FindObjectOfType<DialogueManager>().DialogueLength == dialogue.sentences.Length - 1 && OnDialoguePlay.GetPersistentEventCount() > 0) 
            OnDialoguePlay.Invoke();
    }

    private void FinishInvoke() {
        Debug.Log("GetPersistentEventCount: " + OnDialogueFinish.GetPersistentEventCount());
        if (FindObjectOfType<DialogueManager>().DialogueLength == 0 && OnDialogueFinish.GetPersistentEventCount() > 0) {
            Debug.Log(dialogueManager.DialogueLength);
            OnDialogueFinish.Invoke();
            dialogueManager.DialogueLength = 100;
        }
    }

    // Trigger the dialogue with optional events
    public void TriggerDialogue()
    {
        dialogueManager.DialogueLength = dialogue.sentences.Length-1;
        PlayInvoke();

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);


        FinishInvoke();
    }

    // Trigger the dialogue with automatic next sentence
    public void TriggerDialogueWithAutoNext(float delayTime)
    {
        StopAllCoroutines();

                PlayInvoke();

        dialogueManager.DialogueLength = dialogue.sentences.Length-1;

        FindObjectOfType<DialogueManager>().StartAutoNextSentence(dialogue, delayTime);
        FinishInvoke();
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
