using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonQuestDialogue : MonoBehaviour
{
    [Header("Dialogues")]
    [SerializeField] private List<TextAsset> startDungeonDialogue;
    [SerializeField] private List<TextAsset> progressDialogue;
    [SerializeField] private TextAsset endDungeonDialogue;

    public void StartDungeonDialogue()
    {
        DialogueManager.instance.StartDialogue(startDungeonDialogue[Random.Range(0, startDungeonDialogue.Count)]);
    }
}
