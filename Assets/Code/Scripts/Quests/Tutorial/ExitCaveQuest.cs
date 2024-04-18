using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCaveQuest : QuestStep
{
    public static ExitCaveQuest instance;

    private bool hasExited;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Exit Cave Quests in the scene!");
        }

        else
        {
            instance = this;
        }
    }

    public void ExitCave()
    {
        // make the bear the child of Quest Manager
        //bear.transform.parent = transform.parent;

        Debug.Log("Fetching latest completed dialogue value before leaving the cave: " + ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState("latestTutorialQuestStepDialogueCompleted")).value);

        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = hasExited.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        hasExited = System.Convert.ToBoolean(state);

        UpdateState();
    }

}
