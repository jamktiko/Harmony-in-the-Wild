using UnityEngine.Serialization;

public class CollectableQuestStepBoneToPick : QuestStep
{
    [FormerlySerializedAs("itemsCollected")] public int ItemsCollected = 0;
    private int _itemToComplete = 1;

    public void CollectableProgress()
    {
        ItemsCollected++;
        UpdateState();
        if (ItemsCollected >= _itemToComplete)
        {
            FinishQuestStep();
        }
        //GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");

    }


    private void UpdateState()
    {
        string state = ItemsCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        ItemsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
