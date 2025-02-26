public class DestinationQuestStep : QuestStep
{
    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnReachTargetDestinationToCompleteQuestStep += MarkQuestAsCompleted;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnReachTargetDestinationToCompleteQuestStep -= MarkQuestAsCompleted;
    }

    private void MarkQuestAsCompleted(string id)
    {
        if (id == GetQuestId())
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        // nothing to update
    }

    protected override void SetQuestStepState(string state)
    {
        // nothing to update
    }
}
