using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhaleFinalDestination : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnReachWhaleDestination += CompleteQuestStep;
        SceneManager.sceneLoaded += TriggerMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnReachWhaleDestination -= CompleteQuestStep;
        SceneManager.sceneLoaded -= TriggerMovement;
    }

    private void TriggerMovement(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.questEvents.StartMovingWhale();

            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
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
