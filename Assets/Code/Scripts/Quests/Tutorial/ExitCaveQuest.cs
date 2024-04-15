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
