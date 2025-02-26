using UnityEngine;
using UnityEngine.Serialization;

public class CallToReturnToQuestPoint : MonoBehaviour
{
    [FormerlySerializedAs("questInfoForPoint")] [SerializeField] private QuestScriptableObject _questInfoForPoint;
    [FormerlySerializedAs("finalCallToAction")] [SerializeField] private string _finalCallToAction;
    private string _questId;

    private void Start()
    {
        _questId = _questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnReturnToSideQuestPoint += UpdateQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnReturnToSideQuestPoint -= UpdateQuestUI;
    }

    private void UpdateQuestUI(string id)
    {
        // if the corresponding quest is being updated to CAN_FINISH state, use this UI
        if (_questId == id)
        {
            GameEventsManager.instance.QuestEvents.ShowQuestUI(_questId, _finalCallToAction, "");
        }
    }
}
