using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointDialogue : MonoBehaviour
{
    [Header("Dialogue Files")]
    [SerializeField] private AudioName audioToPlayOnDialogueStarted;
    [SerializeField] private TextAsset requirementsNotMetDialogue;
    [SerializeField] private TextAsset startQuestDialogue;
    [SerializeField] private TextAsset finishQuestDialogue;
    [SerializeField] private TextAsset afterQuestFinishedDialogue;
    [SerializeField] private TextAsset questInProgressDialogue;
    [SerializeField] private List<TextAsset> midQuestDialogues;

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += PreventNewDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= PreventNewDialogue;
    }

    public void RequirementsNotMetDialogue()
    {
        if(requirementsNotMetDialogue != null)
        {
            DialogueManager.instance.StartDialogue(requirementsNotMetDialogue);
        }
    }

    public void StartQuestDialogue()
    {
        if(startQuestDialogue != null)
        {
            DialogueManager.instance.StartDialogue(startQuestDialogue);
            PlayDialogueSound();
        }
    }

    public void FinishQuestDialogue()
    {
        if (finishQuestDialogue != null)
        {
            DialogueManager.instance.StartDialogue(finishQuestDialogue);
            PlayDialogueSound();
        }
    }

    public void AfterQuestFinishedDialogue()
    {
        if (afterQuestFinishedDialogue != null)
        {
            DialogueManager.instance.StartDialogue(afterQuestFinishedDialogue);
            PlayDialogueSound();
        }
    }

    public void MidQuestDialogue(int index)
    {
        if (midQuestDialogues[index] != null)
        {
            DialogueManager.instance.StartDialogue(midQuestDialogues[index]);
            PlayDialogueSound();
        }
    }

    public void QuestInProgressDialogue()
    {
        if(questInProgressDialogue != null)
        {
            DialogueManager.instance.StartDialogue(questInProgressDialogue);
        }
    }

    private void PlayDialogueSound()
    {
        if (DialogueManager.instance.isDialoguePlaying)
        {
            AudioManager.instance.PlaySound(audioToPlayOnDialogueStarted, transform);
        }
    }

    private void PreventNewDialogue()
    {
        //canStartDialogue = false;
        Invoke(nameof(EnableNewDialogue), 2f);
    }

    private void EnableNewDialogue()
    {
        //canStartDialogue = true;
    }
}
