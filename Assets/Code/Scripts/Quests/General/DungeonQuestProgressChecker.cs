using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonQuestProgressChecker : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private Abilities ability;

    private QuestState currentState;

    private void Start()
    {
        currentState = QuestManager.instance.CheckQuestState(quest.id);

        CheckForAbilityLock();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnEndCinematics += MarkQuestAsFinished;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnEndCinematics -= MarkQuestAsFinished;
    }

    private void MarkQuestAsFinished()
    {
        // if quest is ready to be finished, mark it as finished
        if(currentState == QuestState.CAN_FINISH)
        {
            GameEventsManager.instance.questEvents.FinishQuest(quest.id);
        }
    }

    private void CheckForAbilityLock()
    {
        // if quest is still in progress, lock the ability so the player cannot yet use it in Overworld
        if (currentState == QuestState.FINISHED || currentState == QuestState.CAN_FINISH)
        {
            return;
        }

        else
        {
            AbilityManager.instance.LockAbility(ability);
        }
    }
}