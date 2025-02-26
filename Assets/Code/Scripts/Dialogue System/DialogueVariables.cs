using System.Collections.Generic;

// used to track progress in quests where you need to talk to other characters in order to proceed
// please name in the style of <QUEST_NAME>_<QUEST_STEP_INDEX>
public enum DialogueVariables
{
    Tutorial01,
    Tutorial03,
    Tutorial05,
    Tutorial07,
    Tutorial08,
    WhaleDiet02,
    BoneToPick03
}

public static class DialogueVariableInitializer
{
    public static readonly Dictionary<DialogueVariables, bool> InitialVariables = new Dictionary<DialogueVariables, bool>
    {
        {DialogueVariables.Tutorial01, false},
        {DialogueVariables.Tutorial03, false},
        {DialogueVariables.Tutorial05, false},
        {DialogueVariables.Tutorial07, false},
        {DialogueVariables.Tutorial08, false},
        {DialogueVariables.WhaleDiet02, false},
        {DialogueVariables.BoneToPick03, false}
    };
}

public enum DialogueQuestNpCs
{
    Default,
    Bear,
    Whale,
    Wolf
}