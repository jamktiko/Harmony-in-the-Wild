using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJumpQuestStep : QuestStep
{
    [SerializeField] private int jumps = 0;
    [SerializeField] private int jumpsNeeded = 3;

    public void JumpProgress()
    {
        if(jumps < jumpsNeeded)
        {
            jumps++;
            UpdateState();
        }

        if (jumps >= jumpsNeeded)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = jumps.ToString();
        ChangeState(state);
    }
}
