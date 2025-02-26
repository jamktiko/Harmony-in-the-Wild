public class DungeonQuestQuestStep : QuestStep
{
    private string _dungeonQuestId;

    private void Start()
    {
        _dungeonQuestId = GetQuestId();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest += CompleteDungeonQuestStep;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest -= CompleteDungeonQuestStep;
    }

    private void CompleteDungeonQuestStep(string id)
    {
        if (id == _dungeonQuestId)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // this quest step doesn't require anything to be saved, as the progress is being tracked based on the amount of completed quest steps
    }
}