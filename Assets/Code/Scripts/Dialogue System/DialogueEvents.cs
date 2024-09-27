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
}
