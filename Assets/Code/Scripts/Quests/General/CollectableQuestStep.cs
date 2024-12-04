public class CollectableQuestStep : QuestStep
{
    public int itemsCollected = 0;
    private int itemToComplete = 5;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress + " " + itemsCollected + "/" + itemToComplete);
    }

    public void CollectableProgress()
    {
        itemsCollected++;
        UpdateState();
        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI(progress + " " + itemsCollected + "/" + itemToComplete);

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
