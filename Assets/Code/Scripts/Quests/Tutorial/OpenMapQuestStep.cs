using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMapQuestStep : QuestStep
{
    private int mapActionsDone;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        mapActionsDone++;

        // if map has been both opened and closed, progress in the quest
        if(mapActionsDone >= 2)
        {
            FinishQuestStep();
        }

        // otherwise just update the quest state
        else
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        string state = mapActionsDone.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        mapActionsDone = System.Int32.Parse(state);

        UpdateState();
    }

}
