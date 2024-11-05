using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueEvents
{
    public event Action OnStartDialogue;

    public void StartDialogue()
    {
        OnStartDialogue?.Invoke();
    }

    public event Action OnEndDialogue;

    public void EndDialogue()
    {
        OnEndDialogue?.Invoke();
    }

    public event Action<int, string> OnSetMidQuestDialogue;

    public void SetMidQuestDialogue(int dialogueIndex, string questId)
    {
        OnSetMidQuestDialogue?.Invoke(dialogueIndex, questId);
    }
}
