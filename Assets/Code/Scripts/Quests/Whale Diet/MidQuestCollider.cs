using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MidQuestCollider : MonoBehaviour
{
    [FormerlySerializedAs("quest")] [SerializeField] private QuestScriptableObject _quest;

    private QuestState _currentState;
    private List<Transform> _childColliders = new List<Transform>();

    private void Start()
    {
        CheckQuestProgressStatus();

        if (_currentState != QuestState.Finished)
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
        GameEventsManager.instance.QuestEvents.OnFinishQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= UpdateQuestProgressStatus;
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

            if (_currentState == QuestState.Finished && transform.childCount > 0)
            {
                DisableQuestColliders();
            }
        }
    }

    public void AddChildTransformToList(Transform child)
    {
        _childColliders.Add(child);
    }
}
