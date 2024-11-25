using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleFinalDestination : QuestStep
{
    private void Start()
    {
        GameEventsManager.instance.questEvents.StartMovingWhale();

        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnReachWhaleDestination += CompleteQuestStep;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnReachWhaleDestination -= CompleteQuestStep;
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
