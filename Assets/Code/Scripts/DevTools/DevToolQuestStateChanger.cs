using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DevToolQuestStateChanger : MonoBehaviour
{
    [FormerlySerializedAs("quest")] [SerializeField] private QuestScriptableObject _quest;
    [FormerlySerializedAs("dropdown")] [SerializeField] private TMP_Dropdown _dropdown;
    private string _questId;
    private QuestState _currentQuestState;

    private void OnEnable()
    {
        _questId = _quest.id;

        _currentQuestState = QuestManager.Instance.GetQuestById(_questId).State;

        // set the current state of the quest to be shown
        switch (_currentQuestState)
        {
            case QuestState.RequirementsNotMet:
                _dropdown.value = 0;
                break;

            case QuestState.CanStart:
                _dropdown.value = 1;
                break;

            case QuestState.InProgress:
                _dropdown.value = 2;
                break;

            case QuestState.CanFinish:
                _dropdown.value = 3;
                break;

            case QuestState.Finished:
                _dropdown.value = 4;
                break;
        }
    }

    public void ChangeQuestState()
    {
        switch (_dropdown.value)
        {
            case 2:
                GameEventsManager.instance.QuestEvents.StartQuest(_questId);
                break;

            case 4:
                GameEventsManager.instance.QuestEvents.FinishQuest(_questId);
                break;

            default:
                Debug.Log("No changes coded for this state type.");
                break;
        }
    }
}
