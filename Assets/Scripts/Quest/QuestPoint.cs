using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    [Header("Dialogue Config")]
    [SerializeField] private bool hasDialogue;
    [SerializeField] private TextAsset startQuestDialogue;
    [SerializeField] private TextAsset finishQuestDialogue;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            InteractedWithQuestPoint();
        }
    }

    private void InteractedWithQuestPoint()
    {
        if (!playerIsNear)
        {
            Debug.Log("player wasn't near enough");
            return;
        }

        // start or finish a quest
        if(currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            if(hasDialogue && startQuestDialogue != null)
            {
                Debug.Log("starting dialogue");
                DialogueManager.instance.StartDialogue(startQuestDialogue);
            }

            GameEventsManager.instance.questEvents.StartQuest(questId);
        }

        else if(currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            if (hasDialogue && finishQuestDialogue != null)
            {
                DialogueManager.instance.StartDialogue(finishQuestDialogue);
            }

            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
