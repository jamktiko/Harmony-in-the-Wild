using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonQuestDialogue : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private List<TextAsset> startDungeonDialogue;
    [SerializeField] private List<TextAsset> progressDialogue;
    [SerializeField] private TextAsset endDungeonDialogue;

    private bool hadFinalDialogue = false;

    public void PlayStartDungeonDialogue()
    {
        DialogueManager.instance.StartDialogue(startDungeonDialogue[Random.Range(0, startDungeonDialogue.Count)]);

        hadFinalDialogue = true;
    }

    public void PlayFinishDungeonDialogue()
    {
        DialogueManager.instance.StartDialogue(endDungeonDialogue);

        hadFinalDialogue = true;
    }

    public bool FinalDialogueCompleted()
    {
        return hadFinalDialogue;
    }
}
