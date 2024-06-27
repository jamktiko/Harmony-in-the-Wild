using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallToReturnToQuestPoint : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questInfoForPoint;
    [SerializeField] private string finalCallToAction;
    private string questId;

    private void Start()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnReturnToSideQuestPoint += UpdateQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnReturnToSideQuestPoint -= UpdateQuestUI;
    }

    private void UpdateQuestUI(string id)
    {
        // if the corresponding quest is being uopdated to CAN_FINISH state, use this UI
        if(questId == id)
        {
            GameEventsManager.instance.questEvents.ShowQuestUI(questId, finalCallToAction, "");
        }
    }
}
