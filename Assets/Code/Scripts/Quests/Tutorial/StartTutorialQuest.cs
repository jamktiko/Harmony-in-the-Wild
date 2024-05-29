using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorialQuest : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    private void Start()
    {
        if (QuestManager.instance.CheckQuestState(questSO.id) == QuestState.CAN_START)
        {
            Debug.Log("Start tutorial.");
            GameEventsManager.instance.questEvents.StartQuest(questSO.id);
            Destroy(this);
        }

        else
        {
            Debug.Log("Not starting tutorial quest, current state: " + QuestManager.instance.CheckQuestState(questSO.id));
            Destroy(this);
        }
    }
}
