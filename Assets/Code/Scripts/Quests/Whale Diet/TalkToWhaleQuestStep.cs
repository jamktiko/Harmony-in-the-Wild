using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToWhaleQuestStep : QuestStep
{
    [Tooltip("Target index for the completed dialogue. Checking if the dialogue with this quest step has been completed.")]
    [SerializeField] private int targetDialogueIndex;

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += CheckProgressInDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= CheckProgressInDialogue;
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
