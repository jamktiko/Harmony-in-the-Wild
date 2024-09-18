using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStatusChecker : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private Abilities ability;

    private void Start()
    {
        CheckQuestStatus();
    }

    private void CheckQuestStatus()
    {
        QuestState currentState = QuestManager.instance.CheckQuestState(quest.id);

        if(currentState != QuestState.FINISHED)
        {
            AbilityManager.instance.LockAbility(ability);
        }
    }
}
