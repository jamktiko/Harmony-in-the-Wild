using UnityEngine;
using UnityEngine.Serialization;

public class StartTutorialQuest : MonoBehaviour
{
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;

    private void Start()
    {
        if (QuestManager.Instance.CheckQuestState(_questSo.id) == QuestState.CanStart)
        {
            GameEventsManager.instance.QuestEvents.StartQuest(_questSo.id);
            Destroy(this);
        }

        else
        {
            Debug.Log("Not starting tutorial quest, current state: " + QuestManager.Instance.CheckQuestState(_questSo.id));
            Destroy(this);
        }
    }
}
