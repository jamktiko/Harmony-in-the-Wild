using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogLearningQuestStep : QuestStep
{
    bool completed=false;
    // Start is called before the first frame update
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += CompleteDungeonQuestStep;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= CompleteDungeonQuestStep;
    }
    private void CompleteDungeonQuestStep(string id)
    {
     FinishQuestStep();
    }
    // Update is called once per frame
    protected override void SetQuestStepState(string state)
    {
        completed = true;
    }
}
