using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTutorial : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;

    private void Start()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange += FinishTutorialQuest;
    }

    private void FinishTutorialQuest(Quest quest)
    {
        if (quest.info.id.Equals(tutorialQuest.id))
        {
            Debug.Log("Checking whether tutorial can be finished...");

            if(quest.state == QuestState.CAN_FINISH)
            {
                Debug.Log("Tutorial can be finished.");
                GameEventsManager.instance.questEvents.FinishQuest(tutorialQuest.id);
            }

            else
            {
                Debug.Log("Tutorial can't be finished, current state: " + quest.state);
            }
        }
    }
}
