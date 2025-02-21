using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationQuestStep : QuestStep
{
    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnReachTargetDestinationToCompleteQuestStep += MarkQuestAsCompleted;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnReachTargetDestinationToCompleteQuestStep -= MarkQuestAsCompleted;
    }

    private void MarkQuestAsCompleted(string id)
    {
        if(id == GetQuestId())
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
