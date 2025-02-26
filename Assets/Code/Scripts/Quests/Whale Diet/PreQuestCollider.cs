using UnityEngine;
using UnityEngine.Serialization;

public class PreQuestCollider : MonoBehaviour
{
    [FormerlySerializedAs("quest")] [SerializeField] private QuestScriptableObject _quest;

    private QuestState _currentState;

    private void Start()
    {
        CheckQuestProgressStatus();

        if (_currentState == QuestState.RequirementsNotMet || _currentState == QuestState.CanStart)
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
        GameEventsManager.instance.QuestEvents.OnStartQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest -= UpdateQuestProgressStatus;
    }

    private void CheckQuestProgressStatus()
    {
        _currentState = QuestManager.Instance.CheckQuestState(_quest.id);
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
        if (id == _quest.id)
        {
            CheckQuestProgressStatus();

            if ((_currentState != QuestState.RequirementsNotMet || _currentState != QuestState.CanStart) && transform.childCount > 0)
            {
                DisableQuestColliders();
            }
        }
    }
}
