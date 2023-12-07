using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableQuestStep_BoneToPick : QuestStep
{
    public int itemsCollected = 0;
    private int itemToComplete = 1;
    public void CollectableProgress()
    {
        itemsCollected++;
        UpdateState();

        if (itemsCollected >= itemToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = itemsCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        itemsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
