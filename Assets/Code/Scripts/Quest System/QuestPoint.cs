using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [FormerlySerializedAs("questInfoForPoint")]
    [Header("Quest")]
    [SerializeField] public QuestScriptableObject QuestInfoForPoint;

    [FormerlySerializedAs("startPoint")]
    [Header("Config")]
    [SerializeField] private bool _startPoint = true;
    [FormerlySerializedAs("finishPoint")] [SerializeField] private bool _finishPoint = true;
    [FormerlySerializedAs("character")]
    [Tooltip("You only need to set this for the quests that have dialogue related progress. Otherwise can be left as Default.")]
    [SerializeField] private DialogueQuestNpCs _character;

    [FormerlySerializedAs("playerIsNear")]
    [Header("Dialogue Config")]
    [SerializeField] private bool _playerIsNear = false;

    private string _questId;
    private QuestState _currentQuestState;
    private QuestPointDialogue _questPointDialogue;
    private bool _readyToStartQuest;
    private bool _readyToCompleteQuest;
    private bool _midQuestDialogueSet = false;
    private int _midQuestDialogueIndex = 0;

    [FormerlySerializedAs("respawnPoint")]
    [Header("RespawnPoint")]
    [SerializeField] private Transform _respawnPoint;

    private void Awake()
    {
        _questId = QuestInfoForPoint.id;
        _questPointDialogue = GetComponent<QuestPointDialogue>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += StartOrCompleteQuest;
        GameEventsManager.instance.DialogueEvents.OnSetMidQuestDialogue += SetMidQuestDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue -= StartOrCompleteQuest;
        GameEventsManager.instance.DialogueEvents.OnSetMidQuestDialogue -= SetMidQuestDialogue;
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear)
        {
            InteractedWithQuestPoint();
        }
    }

    private void InteractedWithQuestPoint()
    {
        _currentQuestState = QuestManager.Instance.QuestMap[_questId].State;

        // play dialogue if you are not able to start the quest yet
        if (_currentQuestState.Equals(QuestState.RequirementsNotMet) && _startPoint)
        {
            Debug.Log("Interacting with quest point, not ready for this quest yet!");
            _questPointDialogue.RequirementsNotMetDialogue();
        }

        // start or finish a quest
        else if (_currentQuestState.Equals(QuestState.CanStart) && _startPoint)
        {
            Debug.Log("Interacting with quest point, about to start a quest.");
            _readyToStartQuest = true;
            _questPointDialogue.StartQuestDialogue();
        }

        else if (_currentQuestState.Equals(QuestState.CanFinish) && _finishPoint)
        {
            Debug.Log("Interacting with quest point, about to finish a quest.");
            _readyToCompleteQuest = true;
            _questPointDialogue.FinishQuestDialogue();
        }

        // if the quest has already been finished, trigger the default dialogue
        else if (_currentQuestState.Equals(QuestState.Finished))
        {
            Debug.Log("Interacting with quest point, quest has been completed previously.");
            _questPointDialogue.AfterQuestFinishedDialogue();
        }

        else if (_currentQuestState.Equals(QuestState.InProgress) && _midQuestDialogueSet)
        {
            if (_midQuestDialogueSet)
            {
                Debug.Log("Interacting with quest point, about to start a mid quest dialogue.");
                _questPointDialogue.MidQuestDialogue(_midQuestDialogueIndex);
                _midQuestDialogueSet = false;
            }

            else
            {
                Debug.Log("Interacting with quest point, about to start a quest in progress dialogue.");
                _questPointDialogue.QuestInProgressDialogue();
            }
        }

        RespawnManager.Instance.SetRespawnPosition(_respawnPoint.transform.position);
    }

    private void StartOrCompleteQuest()
    {
        if (_playerIsNear && _readyToStartQuest && !_readyToCompleteQuest && _currentQuestState.Equals(QuestState.CanStart))
        {
            GameEventsManager.instance.QuestEvents.StartQuest(_questId);
        }

        else if (_playerIsNear && _readyToCompleteQuest && _currentQuestState.Equals(QuestState.CanFinish))
        {
            GameEventsManager.instance.QuestEvents.FinishQuest(_questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.Info.id.Equals(_questId))
        {
            _currentQuestState = quest.State;
            //Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    public string ReturnQuestId()
    {
        return _questId;
    }

    private void SetMidQuestDialogue(int dialogueIndex, string id)
    {
        if (id == _questId)
        {
            _midQuestDialogueSet = true;
            _midQuestDialogueIndex = dialogueIndex;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = true;

            if (_character != DialogueQuestNpCs.Default)
            {
                GameEventsManager.instance.DialogueEvents.RegisterPlayerNearNpc(_character, _playerIsNear);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = false;

            if (_character != DialogueQuestNpCs.Default)
            {
                GameEventsManager.instance.DialogueEvents.RegisterPlayerNearNpc(_character, _playerIsNear);
            }
        }
    }
}
