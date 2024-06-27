using System.Linq;
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
    [SerializeField] private bool playerIsNear = false;

    private string questId;
    private QuestState currentQuestState;
    private QuestPointDialogue questPointDialogue;

    [Header("RespawnPoint")]
    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questPointDialogue = GetComponent<QuestPointDialogue>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void Update()
    {
        if(PlayerInputHandler.instance.InteractInput.WasPressedThisFrame()&&playerIsNear)
        {
           currentQuestState= QuestManager.instance.questMap[questId].state;
            InteractedWithQuestPoint();
        }
    }

    private void InteractedWithQuestPoint()
    {
            // start or finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                Debug.Log("Quest -1");

                GameEventsManager.instance.questEvents.StartQuest(questId);
                questPointDialogue.StartQuestDialogue();
            }

            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                Debug.Log("Quest 0");

                GameEventsManager.instance.questEvents.FinishQuest(questId);
                questPointDialogue.FinishQuestDialogue();
            }

            // if the quest has already been finished, trigger the default dialogue
            else if (currentQuestState.Equals(QuestState.FINISHED))
            {
                questPointDialogue.AfterQuestFinishedDialogue();
            }

            RespawnManager.instance.SetRespawnPosition(respawnPoint.transform.position);
        }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            //Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    public string ReturnQuestId()
    {
        return questId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }
}
