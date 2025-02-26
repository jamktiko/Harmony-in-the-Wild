using System.Collections.Generic;

// used to track progress in quests where you need to talk to other characters in order to proceed
// please name in the style of <QUEST_NAME>_<QUEST_STEP_INDEX>
public enum DialogueVariables
{
    Tutorial_01,
    Tutorial_03,
    Tutorial_05,
    Tutorial_07,
    Tutorial_08,
    WhaleDiet_02,
    BoneToPick_03
}

public static class DialogueVariableInitializer
{
    public static readonly Dictionary<DialogueVariables, bool> initialVariables = new Dictionary<DialogueVariables, bool>
    {
        {DialogueVariables.Tutorial_01, false},
        {DialogueVariables.Tutorial_03, false},
        {DialogueVariables.Tutorial_05, false},
        {DialogueVariables.Tutorial_07, false},
        {DialogueVariables.Tutorial_08, false},
        {DialogueVariables.WhaleDiet_02, false},
        {DialogueVariables.BoneToPick_03, false}
    };
}

public enum DialogueQuestNPCs
{
    Default,
    Bear,
    Whale,
    Wolf
}