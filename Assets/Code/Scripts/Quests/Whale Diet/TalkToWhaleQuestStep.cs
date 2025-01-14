using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkToWhaleQuestStep : QuestStep
{
    [SerializeField] DialogueVariables dialogueToPassForProgress;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        GameEventsManager.instance.dialogueEvents.SetMidQuestDialogue(0, GetQuestId());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable += CheckProgressInDialogue;
        SceneManager.sceneLoaded += SetInfoInOverworld;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable -= CheckProgressInDialogue;
        SceneManager.sceneLoaded -= SetInfoInOverworld;
    }

    private void SetInfoInOverworld(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.dialogueEvents.SetMidQuestDialogue(0, GetQuestId());
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }
    }

    private void CheckProgressInDialogue(DialogueVariables changedVariable)
    {
        if (changedVariable == dialogueToPassForProgress)
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
