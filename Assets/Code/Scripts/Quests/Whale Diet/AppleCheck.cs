using UnityEngine;
using UnityEngine.Serialization;

public class AppleCheck : MonoBehaviour
{
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;

    private void Start()
    {
        QuestState questState = QuestManager.Instance.CheckQuestState(_questSo.id);

        if (questState == QuestState.Finished || QuestManager.Instance.GetQuestById(_questSo.id).GetCurrentQuestStepIndex() > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
