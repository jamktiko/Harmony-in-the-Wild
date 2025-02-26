using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestPointDialogue : MonoBehaviour
{
    [FormerlySerializedAs("audioToPlayOnDialogueStarted")]
    [Header("Dialogue Files")]
    [SerializeField] private AudioName _audioToPlayOnDialogueStarted;
    [FormerlySerializedAs("requirementsNotMetDialogue")] [SerializeField] private TextAsset _requirementsNotMetDialogue;
    [FormerlySerializedAs("startQuestDialogue")] [SerializeField] private TextAsset _startQuestDialogue;
    [FormerlySerializedAs("finishQuestDialogue")] [SerializeField] private TextAsset _finishQuestDialogue;
    [FormerlySerializedAs("afterQuestFinishedDialogue")] [SerializeField] private TextAsset _afterQuestFinishedDialogue;
    [FormerlySerializedAs("questInProgressDialogue")] [SerializeField] private TextAsset _questInProgressDialogue;
    [FormerlySerializedAs("midQuestDialogues")] [SerializeField] private List<TextAsset> _midQuestDialogues;

    private void OnEnable()
    {
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += PreventNewDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.DialogueEvents.OnEndDialogue -= PreventNewDialogue;
    }

    public void RequirementsNotMetDialogue()
    {
        if (_requirementsNotMetDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_requirementsNotMetDialogue);
        }
    }

    public void StartQuestDialogue()
    {
        if (_startQuestDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_startQuestDialogue);
            PlayDialogueSound();
        }
    }

    public void FinishQuestDialogue()
    {
        if (_finishQuestDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_finishQuestDialogue);
            PlayDialogueSound();
        }
    }

    public void AfterQuestFinishedDialogue()
    {
        if (_afterQuestFinishedDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_afterQuestFinishedDialogue);
            PlayDialogueSound();
        }
    }

    public void MidQuestDialogue(int index)
    {
        if (_midQuestDialogues[index] != null)
        {
            DialogueManager.Instance.StartDialogue(_midQuestDialogues[index]);
            PlayDialogueSound();
        }
    }

    public void QuestInProgressDialogue()
    {
        if (_questInProgressDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_questInProgressDialogue);
        }
    }

    private void PlayDialogueSound()
    {
        if (DialogueManager.Instance.IsDialoguePlaying)
        {
            AudioManager.Instance.PlaySound(_audioToPlayOnDialogueStarted, transform);
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
