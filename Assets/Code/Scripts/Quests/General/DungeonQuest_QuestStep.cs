using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonQuest_QuestStep : QuestStep
{
    private string dungeonQuestId;

    private void Start()
    {
        dungeonQuestId = GetQuestId();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest += CompleteDungeonQuestStep;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest -= CompleteDungeonQuestStep;
    }
    
    private void CompleteDungeonQuestStep(string id)
    {
        if(id == dungeonQuestId)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // this quest step doesn't require anything to be saved, as the progress is being tracked based on the amount of completed quest steps
    }
}