using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TutorialBear : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject questInfoForPoint;

    [Header("Dialogue Files")]
    [SerializeField] private List<TextAsset> dialogueFiles;

    private int dialogueTracker = 0; // the index of the latest completed dialogue; will help in triggering the next dialogue after the previous one has been completed
    
    private string questId;
    private bool playerIsNear = false;
    private AudioSource audioSource;

    private const string latestCompletedDialogue = "latestTutorialQuestStepDialogueCompleted";

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {
            InteractWithBear();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            dialogueTracker = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState(latestCompletedDialogue)).value;
        }
    }

    private void InteractWithBear()
    {
        // fetch correct dialogue index
        dialogueTracker = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState(latestCompletedDialogue)).value;

        DialogueManager.instance.StartDialogue(dialogueFiles[dialogueTracker]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }
}
