using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPointDialogue : MonoBehaviour
{
    [Header("Dialogue Files")]
    [SerializeField] private TextAsset startQuestDialogue;
    [SerializeField] private TextAsset finishQuestDialogue;
    [SerializeField] private TextAsset afterQuestFinishedDialogue;

    private AudioSource audioSource;
    private bool canStartDialogue = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += PreventNewDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= PreventNewDialogue;
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

    private void PlayDialogueSound()
    {
        audioSource.Play();
    }

    private void PreventNewDialogue()
    {
        canStartDialogue = false;
        Invoke(nameof(EnableNewDialogue), 2f);
    }

    private void EnableNewDialogue()
    {
        canStartDialogue = true;
    }
}
