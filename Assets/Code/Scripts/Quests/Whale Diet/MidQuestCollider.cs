using System.Collections.Generic;
using UnityEngine;

public class MidQuestCollider : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;

    private QuestState currentState;
    private List<Transform> childColliders = new List<Transform>();

    private void Start()
    {
        CheckQuestProgressStatus();

        if (currentState != QuestState.FINISHED)
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
        GameEventsManager.instance.questEvents.OnFinishQuest += UpdateQuestProgressStatus;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest -= UpdateQuestProgressStatus;
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

            if (currentState == QuestState.FINISHED && transform.childCount > 0)
            {
                DisableQuestColliders();
            }
        }
    }

    public void AddChildTransformToList(Transform child)
    {
        childColliders.Add(child);
    }
}
