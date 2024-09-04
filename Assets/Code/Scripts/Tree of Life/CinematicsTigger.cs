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
        CheckForTreeOfLifeUpdate();
    }

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

            if(questState == QuestState.FINISHED)
            {
                questsCompleted++;
            }
        }
    }
}
