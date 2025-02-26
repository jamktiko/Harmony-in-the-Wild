using UnityEngine;
using UnityEngine.Serialization;

public class EnableBear : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuest")] [SerializeField] private QuestScriptableObject _tutorialQuest;

    private void Start()
    {
        QuestState currentState = QuestManager.Instance.CheckQuestState(_tutorialQuest.id);

        // if quest is finished, keep the bear enabled
        if (currentState == QuestState.Finished)
        {
            return;
        }

        // otherwise disable this bear, since there is another one controlled by Quest Manager
        else
        {
            Transform questManager = QuestManager.Instance.transform;

            foreach (Transform child in questManager)
            {
                if (child.name.Contains("Bear"))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}