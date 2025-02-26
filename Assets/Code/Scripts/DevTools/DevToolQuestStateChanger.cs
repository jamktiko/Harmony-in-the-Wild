using TMPro;
using UnityEngine;

public class DevToolQuestStateChanger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private TMP_Dropdown dropdown;
    private string questId;
    private QuestState currentQuestState;

    private void OnEnable()
    {
        questId = quest.id;

        currentQuestState = QuestManager.instance.GetQuestById(questId).state;

        // set the current state of the quest to be shown
        switch (currentQuestState)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                dropdown.value = 0;
                break;

            case QuestState.CAN_START:
                dropdown.value = 1;
                break;

            case QuestState.IN_PROGRESS:
                dropdown.value = 2;
                break;

            case QuestState.CAN_FINISH:
                dropdown.value = 3;
                break;

            case QuestState.FINISHED:
                dropdown.value = 4;
                break;
        }
    }

    public void ChangeQuestState()
    {
        switch (dropdown.value)
        {
            case 2:
                GameEventsManager.instance.questEvents.StartQuest(questId);
                break;

            case 4:
                GameEventsManager.instance.questEvents.FinishQuest(questId);
                break;

            default:
                Debug.Log("No changes coded for this state type.");
                break;
        }
    }
}
