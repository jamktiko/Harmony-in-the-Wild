using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonQuestDialogue : MonoBehaviour
{
    [FormerlySerializedAs("startDungeonDialogue")]
    [Header("Dialogues")]
    [SerializeField] private List<TextAsset> _startDungeonDialogue;
    [FormerlySerializedAs("progressDialogue")] [SerializeField] private List<TextAsset> _progressDialogue;
    [FormerlySerializedAs("endDungeonDialogue")] [SerializeField] private TextAsset _endDungeonDialogue;

    [FormerlySerializedAs("audioToPlayOnDialogueStarted")]
    [Header("Audio Config")]
    [SerializeField] private AudioName _audioToPlayOnDialogueStarted;

    private bool _hadFinalDialogue = false;

    public void PlayStartDungeonDialogue()
    {
        DialogueManager.Instance.StartDialogue(_startDungeonDialogue[Random.Range(0, _startDungeonDialogue.Count)]);
        AudioManager.Instance.PlaySound(_audioToPlayOnDialogueStarted, transform);
    }

    public void PlayFinishDungeonDialogue()
    {
        DialogueManager.Instance.StartDialogue(_endDungeonDialogue);
        AudioManager.Instance.PlaySound(_audioToPlayOnDialogueStarted, transform);

        _hadFinalDialogue = true;
    }

    public bool FinalDialogueCompleted()
    {
        return _hadFinalDialogue;
    }
}
