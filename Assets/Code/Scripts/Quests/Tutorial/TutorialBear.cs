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

    private bool isInteractable;
    private int dialogueTracker = 0; // the index of the latest completed dialogue; will help in triggering the next dialogue after the previous one has been completed
    private int targetDialogueIndex = 0;
    private bool inkValueUpToDate; // bool to help updating the ink values as they are not currently saved anywhere else; ducktape solution for now

    private string questId;
    private bool playerIsNear = false;
    private AudioSource audioSource;

    private const string latestCompletedDialogue = "latestTutorialQuestStepDialogueCompleted";

    private void Awake()
    {
        questId = questInfoForPoint.id;
        UpdateTargetDialogueIndex(questId);
        InitializeDialogueTracker();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += UpdateTargetDialogueIndex;
        GameEventsManager.instance.dialogueEvents.OnStartDialogue += ToggleInteraction;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += ToggleInteraction;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= UpdateTargetDialogueIndex;
        GameEventsManager.instance.dialogueEvents.OnStartDialogue -= ToggleInteraction;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= ToggleInteraction;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {
            InteractWithBear();
        }
    }

    private void InteractWithBear()
    {
        if (inkValueUpToDate)
        {
            // fetch correct dialogue index
            dialogueTracker = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState(latestCompletedDialogue)).value;

            if (targetDialogueIndex == dialogueTracker)
            {
                DialogueManager.instance.StartDialogue(dialogueFiles[dialogueTracker]);
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
            if (targetDialogueIndex == dialogueTracker)
            {
                DialogueManager.instance.StartDialogue(dialogueFiles[dialogueTracker]);
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

    private void UpdateTargetDialogueIndex(string updatedQuestId)
    {
        if(updatedQuestId == questId)
        {
            int currentQuestStepIndex = QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex();

            switch (currentQuestStepIndex)
            {
                case 2:
                    targetDialogueIndex = 1;
                    break;

                //case 
            }
        }
    }

    private void InitializeDialogueTracker()
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questId).GetCurrentQuestStepIndex();

        switch (currentQuestStepIndex)
        {
            case 0:
                dialogueTracker = 0;
                break;

            case 1:
                dialogueTracker = 1;
                break;

            case 2:
                dialogueTracker = 1;
                break;

            //case 3:
        }
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
