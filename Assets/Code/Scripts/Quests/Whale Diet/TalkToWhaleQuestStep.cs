using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkToWhaleQuestStep : QuestStep
{
    [Tooltip("Target index for the completed dialogue. Checking if the dialogue with this quest step has been completed.")]
    [SerializeField] private int targetDialogueIndex;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        GameEventsManager.instance.dialogueEvents.SetMidQuestDialogue(0, GetQuestId());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += CheckProgressInDialogue;
        SceneManager.sceneLoaded += SetInfoInOverworld;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= CheckProgressInDialogue;
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

    private void CheckProgressInDialogue()
    {
        Invoke(nameof(FetchDialogueData), 0.01f);
    }

    private void FetchDialogueData()
    {
        // check the latest completed dialogue from Ink
        int latestCompletedDialogue = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState("latestWhaleQuestStepDialogueCompleted")).value;

        Debug.Log("Latest completed dialogue with the whale: " + latestCompletedDialogue + ", target dialogue: " + targetDialogueIndex);

        // if the current value matches the target value, finish the quest step
        if (latestCompletedDialogue == targetDialogueIndex)
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
