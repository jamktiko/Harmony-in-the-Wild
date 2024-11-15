using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsTigger : MonoBehaviour
{
    [Tooltip("The main quests; to check if any progress has been made to the previous enter in Overworld")]
    [SerializeField] private List<QuestScriptableObject> questsToCheck;

    private int questsCompleted = 0;
    private int currentTreeOfLifeState;

    private void Start()
    {
        Invoke(nameof(CheckForTreeOfLifeUpdate), 0.3f);
    }

    // Jutta's testing thingies; please don't delete yet, I think things might break later and this is a easy way to keep testing fixes quickly :)
    /*private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest += DebugCinematics;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest -= DebugCinematics;
    }

    private void DebugCinematics(string id)
    {
        if(id == "Whale Diet")
        {
            GameEventsManager.instance.cinematicsEvents.StartCinematics();

            Debug.Log("Trigger cinematics now!");
        }
    }*/

    private void CheckForTreeOfLifeUpdate()
    {
        FetchQuestProgress();

        currentTreeOfLifeState = TreeOfLifeState.instance.GetTreeOfLifeState();

        if(questsCompleted > currentTreeOfLifeState)
        {
            GameEventsManager.instance.cinematicsEvents.StartCinematics();

            Debug.Log("Trigger cinematics now!");
        }
    }

    private void FetchQuestProgress()
    {
        foreach (QuestScriptableObject quest in questsToCheck)
        {
            QuestState questState = QuestManager.instance.CheckQuestState(quest.id);

            if(questState == QuestState.FINISHED || questState == QuestState.CAN_FINISH)
            {
                questsCompleted++;
            }
        }
    }
}
