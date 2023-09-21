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

        if (itemsCollected >= itemToComplete)
        {
            FinishQuestStep();
        }
    }
}
