using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogLearningQuestStep : QuestStep
{
    [SerializeField] int roomnumber;
    // Start is called before the first frame update
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest += FinishFrogQuestStep;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest -= FinishFrogQuestStep;
    }

    private void FinishFrogQuestStep(string id,int stageIndex) 
    {
        if (roomnumber==stageIndex)
        {
            FinishQuestStep();
        }
    }
    // Update is called once per frame
    protected override void SetQuestStepState(string state)
    {
    }
}
