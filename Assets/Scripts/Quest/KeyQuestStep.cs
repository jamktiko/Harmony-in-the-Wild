using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyQuestStep : QuestStep
{
    [SerializeField] int number;
    public int itemsCollected = 0;
    private int itemToComplete = 5;

    void Start()
    {
        if (GameObject.Find("Key" + number).GetComponent<InteractableKey>().wasUsed) 
        {
            CollectableProgress();

        }
    }

    public void CollectableProgress()
    {
        itemsCollected++;
        UpdateState();
        // this should update the quest UI counter for the gathered keys
        GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.UpdateCounter, "Keys gathered " + itemsCollected + "/" + itemToComplete);

        if (itemsCollected >= itemToComplete)
        {
            // this should update the quest UI to show the second objective
            GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
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
