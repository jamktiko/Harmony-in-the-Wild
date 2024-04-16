using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TutorialBear : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject questInfoForPoint;

    [Header("Dialogue Files")]
    [SerializeField] private TextAsset dialogueBetweenQuests;
    [SerializeField] private List<TextAsset> dialogueFiles;

    private bool isInteractable = true;
    private int latestCompletedDialogueIndex = 0; // the index of the latest completed dialogue; will help in triggering the next dialogue after the previous one has been completed
    private int currentDialogueIndex = 0;
    private bool inkValueUpToDate; // bool to help updating the ink values as they are not currently saved anywhere else; ducktape solution for now

    private string questId;
    private bool playerIsNear = false;
    private AudioSource audioSource;

    private const string latestCompletedDialogue = "latestTutorialQuestStepDialogueCompleted";

    private void Start()
    {
        questId = questInfoForPoint.id;
        CheckDialogueProgressChanges(questId);
        InitializeDialogueTracker();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += CheckDialogueProgressChanges;
        GameEventsManager.instance.dialogueEvents.OnStartDialogue += ToggleInteraction;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += ToggleInteraction;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= CheckDialogueProgressChanges;
        GameEventsManager.instance.dialogueEvents.OnStartDialogue -= ToggleInteraction;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= ToggleInteraction;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear && isInteractable)
        {
            InteractWithBear();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("QUI: " + QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex());
        }
    }

    private void InteractWithBear()
    {
        if (inkValueUpToDate)
        {
            // fetch correct dialogue index
            latestCompletedDialogueIndex = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState(latestCompletedDialogue)).value;

            if (currentDialogueIndex == latestCompletedDialogueIndex)
            {
                DialogueManager.instance.StartDialogue(dialogueFiles[latestCompletedDialogueIndex]);
                audioSource.Play();
            }

            else
            {
                DialogueManager.instance.StartDialogue(dialogueBetweenQuests);
                audioSource.Play();
            }
        }

        else
        {
            if (currentDialogueIndex == latestCompletedDialogueIndex)
            {
                DialogueManager.instance.StartDialogue(dialogueFiles[latestCompletedDialogueIndex]);
                inkValueUpToDate = true;
                audioSource.Play();
            }

            else
            {
                DialogueManager.instance.StartDialogue(dialogueBetweenQuests);
                audioSource.Play();
            }
        }
    }

    private void CheckDialogueProgressChanges(string updatedQuestId)
    {
        if(updatedQuestId == questId)
        {
            Invoke(nameof(UpdateDialogueProgressValues), 0.5f);
        }
    }

    private void UpdateDialogueProgressValues()
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex();

        switch (currentQuestStepIndex)
        {
            case 0:
                currentDialogueIndex = 0;
                break;

            case 1:
                currentDialogueIndex = 1;
                break;

            case 2:
                currentDialogueIndex = 1;
                break;

            case 3:
                currentDialogueIndex = 2;
                break;

            case 4:
                currentDialogueIndex = 2;
                break;

            case 5:
                currentDialogueIndex = 3;
                break;

            case 6:
                currentDialogueIndex = 3;
                break;
        }

        if (inkValueUpToDate)
        {
            latestCompletedDialogueIndex = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState(latestCompletedDialogue)).value;
        }

        Debug.Log("Target dialogue index: " + currentDialogueIndex + ", QUI: " + QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex());
    }

    private void InitializeDialogueTracker()
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex();

        switch (currentQuestStepIndex)
        {
            case 0:
                latestCompletedDialogueIndex = 0;
                break;

            case 1:
                latestCompletedDialogueIndex = 1;
                break;

            case 2:
                latestCompletedDialogueIndex = 1;
                break;

            case 3:
                latestCompletedDialogueIndex = 2;
                break;

            case 4:
                latestCompletedDialogueIndex = 2;
                break;

            case 5:
                latestCompletedDialogueIndex = 3;
                break;
        }

        Debug.Log("Initializing latest completed dialogue to: " + latestCompletedDialogueIndex);
    }

    private void ToggleInteraction()
    {
        isInteractable = !isInteractable;
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