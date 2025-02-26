using UnityEngine;

public class EnableBear : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;

    private void Start()
    {
        QuestState currentState = QuestManager.instance.CheckQuestState(tutorialQuest.id);

        // if quest is finished, keep the bear enabled
        if (currentState == QuestState.FINISHED)
        {
            return;
        }

        // otherwise disable this bear, since there is another one controlled by Quest Manager
        else
        {
            Transform questManager = QuestManager.instance.transform;

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