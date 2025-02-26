using UnityEngine;
using UnityEngine.Serialization;

public class FinishTutorial : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuest")] [SerializeField] private QuestScriptableObject _tutorialQuest;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.OnQuestStateChange += FinishTutorialQuest;
    }

    private void FinishTutorialQuest(Quest quest)
    {
        if (quest.Info.id.Equals(_tutorialQuest.id))
        {
            //Debug.Log("Checking whether tutorial can be finished...");

            if (quest.State == QuestState.CanFinish)
            {
                //Debug.Log("Tutorial can be finished.");
                GameEventsManager.instance.QuestEvents.FinishQuest(_tutorialQuest.id);
                SteamManager.Instance.UnlockAchievement("TUTORIAL_ACH");
            }

            else
            {
                //Debug.Log("Tutorial can't be finished, current state: " + quest.state);
            }
        }
    }
}
