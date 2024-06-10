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
    private int currentDialogueIndex = -1;
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
    }

    private void InteractWithBear()
    {
        if (dialogueFiles[currentDialogueIndex] != null)
        {
            DialogueManager.instance.StartDialogue(dialogueFiles[currentDialogueIndex]);
        }

        else
        {
            DialogueManager.instance.StartDialogue(dialogueBetweenQuests);
        }

        audioSource.Play();
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
        currentDialogueIndex++;
    }

    private void InitializeDialogueTracker()
    {
        currentDialogueIndex = QuestManager.instance.GetQuestById(questInfoForPoint.id).GetCurrentQuestStepIndex() - 1;
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