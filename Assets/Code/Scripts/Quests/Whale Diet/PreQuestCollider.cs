using UnityEngine;

public class PreQuestCollider : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;

    private QuestState currentState;

    private void Start()
    {
        CheckQuestProgressStatus();

        if (currentState == QuestState.REQUIREMENTS_NOT_MET || currentState == QuestState.CAN_START)
        {
            return;
        }

        else
        {
            DisableQuestColliders();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest -= UpdateQuestProgressStatus;
    }

    private void CheckQuestProgressStatus()
    {
        currentState = QuestManager.instance.CheckQuestState(quest.id);
    }

    private void DisableQuestColliders()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void UpdateQuestProgressStatus(string id)
    {
        if (id == quest.id)
        {
            CheckQuestProgressStatus();

            if ((currentState != QuestState.REQUIREMENTS_NOT_MET || currentState != QuestState.CAN_START) && transform.childCount > 0)
            {
                DisableQuestColliders();
            }
        }
    }
}
