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

    public event Action<DialogueVariables> OnChangeDialogueVariable;

    public void ChangeDialogueVariable(DialogueVariables changedVariable)
    {
        OnChangeDialogueVariable?.Invoke(changedVariable);
    }

    public event Action<DialogueQuestNPCs, bool> OnRegisterPlayerNearNPC;

    public void RegisterPlayerNearNPC(DialogueQuestNPCs npc, bool isClose)
    {
        OnRegisterPlayerNearNPC?.Invoke(npc, isClose);
    }
}
