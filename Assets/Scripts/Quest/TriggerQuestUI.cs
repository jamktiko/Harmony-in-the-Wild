using UnityEngine;

public class TriggerQuestUI : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int questIndex;

    private QuestState currentState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            currentState = QuestManager.instance.CheckQuestState(questSO.id);

            if(currentState == QuestState.IN_PROGRESS)
            {
                //GameEventsManager.instance.questEvents.ShowQuestUI(questIndex);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            GameEventsManager.instance.questEvents.HideQuestUI();
        }
    }
}
