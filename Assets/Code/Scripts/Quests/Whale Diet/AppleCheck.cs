using UnityEngine;

public class AppleCheck : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    private void Start()
    {
        QuestState questState = QuestManager.instance.CheckQuestState(questSO.id);

        if (questState == QuestState.FINISHED || QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex() > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
