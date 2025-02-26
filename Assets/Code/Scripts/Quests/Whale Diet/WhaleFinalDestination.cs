using UnityEngine.SceneManagement;

public class WhaleFinalDestination : QuestStep
{
    private void Start()
    {
        GameEventsManager.instance.QuestEvents.StartMovingQuestNpc(DialogueQuestNpCs.Whale);

        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnReachWhaleDestination += CompleteQuestStep;
        SceneManager.sceneLoaded += TriggerMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnReachWhaleDestination -= CompleteQuestStep;
        SceneManager.sceneLoaded -= TriggerMovement;
    }

    private void TriggerMovement(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.QuestEvents.StartMovingQuestNpc(DialogueQuestNpCs.Whale);

            GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
        }
    }

    private void CompleteQuestStep()
    {
        FinishQuestStep();
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
