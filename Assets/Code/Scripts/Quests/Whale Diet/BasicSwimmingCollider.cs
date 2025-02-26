using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwimmingCollider : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;

    private QuestState currentState;

    private void Start()
    {
        CheckQuestProgressStatus();

        if (currentState != QuestState.FINISHED)
        {
            return;
        }

        else
        {
            DisableQuestColliders();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest -= UpdateQuestProgressStatus;
    }

    private void CheckQuestProgressStatus()
    {
        currentState = QuestManager.instance.CheckQuestState(quest.id);
    }

    private void DisableQuestColliders()
    {
        gameObject.SetActive(false);
    }

    private void UpdateQuestProgressStatus(string id)
    {
        if (id == quest.id)
        {
            CheckQuestProgressStatus();

            if (currentState == QuestState.FINISHED)
            {
                DisableQuestColliders();
            }
        }
    }
}
