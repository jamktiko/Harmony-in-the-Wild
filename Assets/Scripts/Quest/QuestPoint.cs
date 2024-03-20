using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    [Tooltip("The index of the child object that will be enabled from Quest Canvas adter activating the quest. Set to -1 if no UI is needed.")]
    [SerializeField] private int canvasObjectIndex;

    [Header("Dialogue Config")]
    [SerializeField] private bool hasDialogue;
    [SerializeField] private TextAsset startQuestDialogue;
    [SerializeField] private TextAsset finishQuestDialogue;

    [Header("Dialogue Config")]
    [SerializeField] AudioSource dialogueSound;
    [SerializeField] private bool playerIsNear = false;

    private string questId;
    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange    += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange -= QuestStateChange;
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
            return;
        }

        // start or finish a quest
        if(currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            Debug.Log("Quest -1");
            if(hasDialogue && startQuestDialogue != null)
            {
                DialogueManager.instance.StartDialogue(startQuestDialogue);
                dialogueSound.Play();
            }

            GameEventsManager.instance.questEvents.StartQuest(questId);

            if(canvasObjectIndex >= 0)
            {
                GameObject.FindGameObjectWithTag("QuestCanvas").transform.GetChild(canvasObjectIndex).gameObject.SetActive(true);
            }
        }
        else if(currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            Debug.Log("Quest 0");
            if (hasDialogue && finishQuestDialogue != null)
            {
                Debug.Log("Quest 1");
                DialogueManager.instance.StartDialogue(finishQuestDialogue);
            }

            GameEventsManager.instance.questEvents.FinishQuest(questId);

            if (canvasObjectIndex >= 0)
            {

                Debug.Log("Qeust 2");
                GameObject.FindGameObjectWithTag("QuestCanvas").transform.GetChild(canvasObjectIndex).gameObject.SetActive(false);
            }
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
