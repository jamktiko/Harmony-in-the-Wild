using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToBearQuestStep : QuestStep
{
    [Tooltip("Index of the current quest step. Checking if the dialogue with this quest step has been completed.")]
    [SerializeField] private int dialogueIndex;

    private bool talkedToBear;



    private void UpdateState()
    {
        string state = talkedToBear.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        talkedToBear = System.Convert.ToBoolean(state);

        UpdateState();
    }
}