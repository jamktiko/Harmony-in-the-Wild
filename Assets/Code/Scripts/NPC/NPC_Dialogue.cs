using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour
{
    [SerializeField] private List<NPCQuestDialoguePair> questDialoguePairs;
    [SerializeField] private TextAsset defaultDialogue;

    private AudioSource audioSource;
    private List<TextAsset> possibleDialogues = new List<TextAsset>();
    private bool playerIsNear;

    private void Start()
    {
        Invoke(nameof(CreateListOfPossibleDialogues), 1f);
    }

    private void Update()
    {
        if(PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && playerIsNear)
        {
            SetDialogueToPlay();
        }
    }

    private void SetDialogueToPlay()
    {
        // fetch a random dialogue from the list of possible dialogues
        int dialogueIndex = Random.Range(0, possibleDialogues.Count);

        Debug.Log("Ready to start a NPC dialogue.");

        if(audioSource != null)
        {
            audioSource.Play();
        }

        else
        {
            Debug.LogWarning("No audio source attached to " + gameObject.name + ", cannot play dialogue sound.");
        }

        DialogueManager.instance.StartDialogue(possibleDialogues[dialogueIndex]);
    }

    private void CreateListOfPossibleDialogues()
    {
        // go through the main quests and add the possible dialogues to the list
        foreach(NPCQuestDialoguePair questDialoguePair in questDialoguePairs)
        {
            QuestState state = QuestManager.instance.CheckQuestState(questDialoguePair.mainQuest.id);

            if(state == QuestState.FINISHED)
            {
                possibleDialogues.Add(questDialoguePair.dialogueOption);
            }
        }

        // if none of the main quests have been completed yet, add the default dialogue as an option
        if(possibleDialogues.Count == 0)
        {
            possibleDialogues.Add(defaultDialogue);
        }
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
