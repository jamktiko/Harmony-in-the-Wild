using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CinematicsTigger : MonoBehaviour
{
    [FormerlySerializedAs("questsToCheck")]
    [Tooltip("The main quests; to check if any progress has been made to the previous enter in Overworld")]
    [SerializeField] private List<QuestScriptableObject> _questsToCheck;

    private int _questsCompleted = 0;
    private int _currentTreeOfLifeState;

    private void Start()
    {
        CheckForTreeOfLifeUpdate();
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

        _currentTreeOfLifeState = TreeOfLifeState.Instance.GetTreeOfLifeState();

        if (_questsCompleted > _currentTreeOfLifeState)
        {
            GameEventsManager.instance.CinematicsEvents.StartCinematics();
            AudioManager.Instance.StartNewTheme(ThemeName.ThemeToLCinematics);

            Debug.Log("Trigger cinematics now!");
        }

        else
        {
            Debug.Log("No progress in main quests, no need to trigger cinematics.");
        }
    }

    private void FetchQuestProgress()
    {
        foreach (QuestScriptableObject quest in _questsToCheck)
        {
            QuestState questState = QuestManager.Instance.CheckQuestState(quest.id);

            if (questState == QuestState.Finished || questState == QuestState.CanFinish)
            {
                _questsCompleted++;
            }
        }
    }
}
