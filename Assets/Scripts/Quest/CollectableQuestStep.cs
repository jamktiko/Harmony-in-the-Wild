using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableQuestStep : QuestStep
{
    public int itemsCollected =0;
    private int itemToComplete =3;
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
}
