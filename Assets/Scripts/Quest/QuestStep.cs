using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished =false;

    private string questId;

    private int stepIndex;

    public string stepName;

    public string objective;

    public string progress;
    public void InitializeQuestStep(string id, int stepIndex, string questStepState)
    {
        questId = id;
        this.stepIndex = stepIndex;

        if(questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }

        SaveManager.instance.SaveGame();
    }

    protected string GetQuestId()
    {
        return questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished) 
        { 
            isFinished = true;

            GameEventsManager.instance.questEvents.AdvanceQuest(questId);
            Debug.Log("Finished quest step: " + questId);
            Invoke("DestroyObject",0);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
        SaveManager.instance.SaveGame();
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected abstract void SetQuestStepState(string state);
}
