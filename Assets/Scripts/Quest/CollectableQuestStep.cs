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
        GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.UpdateCounter, "Apples " + itemsCollected + "/" + itemToComplete);

        if (itemsCollected >= itemToComplete)
        {
            FinishQuestStep();
            GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
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
