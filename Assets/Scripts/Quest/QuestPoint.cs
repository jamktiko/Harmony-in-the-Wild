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
    private bool readyToCompleteQuest;

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
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += CompleteQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= CompleteQuest;
    }

    private void Update()
    {
        if(PlayerInputHandler.instance.InteractInput.WasPressedThisFrame()&&playerIsNear)
        {
            InteractedWithQuestPoint();
        }
    }

    private void InteractedWithQuestPoint()
    {
        currentQuestState = QuestManager.instance.questMap[questId].state;

        // play dialogue if you are not able to start the quest yet
        if (currentQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET) && startPoint)
        {
            Debug.Log("Interacting with quest point, not ready for this quest yet!");
            questPointDialogue.RequirementsNotMetDialogue();
        }

        // start or finish a quest
        else if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            Debug.Log("Interacting with quest point, about to start a quest.");
            GameEventsManager.instance.questEvents.StartQuest(questId);
            questPointDialogue.StartQuestDialogue();
        }

        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            Debug.Log("Interacting with quest point, about to finish a quest.");
            readyToCompleteQuest = true;
            questPointDialogue.FinishQuestDialogue();
        }

         // if the quest has already been finished, trigger the default dialogue
         else if (currentQuestState.Equals(QuestState.FINISHED))
         {
            Debug.Log("Interacting with quest point, quest has been completed previously.");
            questPointDialogue.AfterQuestFinishedDialogue();
         }

         RespawnManager.instance.SetRespawnPosition(respawnPoint.transform.position);
    }

    private void CompleteQuest()
    {
        if (playerIsNear && readyToCompleteQuest && currentQuestState.Equals(QuestState.CAN_FINISH))
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
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
