using System.Collections.Generic;
using UnityEngine;

public class DungeonQuestDialogue : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private List<TextAsset> startDungeonDialogue;
    [SerializeField] private List<TextAsset> progressDialogue;
    [SerializeField] private TextAsset endDungeonDialogue;

    [Header("Audio Config")]
    [SerializeField] private AudioName audioToPlayOnDialogueStarted;

    private bool hadFinalDialogue = false;

    public void PlayStartDungeonDialogue()
    {
        DialogueManager.instance.StartDialogue(startDungeonDialogue[Random.Range(0, startDungeonDialogue.Count)]);
        AudioManager.Instance.PlaySound(audioToPlayOnDialogueStarted, transform);
    }

    public void PlayFinishDungeonDialogue()
    {
        DialogueManager.instance.StartDialogue(endDungeonDialogue);
        AudioManager.Instance.PlaySound(audioToPlayOnDialogueStarted, transform);

        hadFinalDialogue = true;
    }

    public bool FinalDialogueCompleted()
    {
        return hadFinalDialogue;
    }
}
