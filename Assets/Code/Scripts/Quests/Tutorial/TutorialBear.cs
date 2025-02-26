using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class TutorialBear : MonoBehaviour
{
    [FormerlySerializedAs("questInfoForPoint")]
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject _questInfoForPoint;

    [FormerlySerializedAs("audioToPlayOnDialogueStarted")]
    [Header("Dialogue Files")]
    [SerializeField] private AudioName _audioToPlayOnDialogueStarted;
    [FormerlySerializedAs("dialogueBetweenQuests")] [SerializeField] private TextAsset _dialogueBetweenQuests;
    [FormerlySerializedAs("dialogueFiles")] [SerializeField] private List<TextAsset> _dialogueFiles;

    private bool _isInteractable = true;
    //private int latestCompletedDialogueIndex = 0; // the index of the latest completed dialogue; will help in triggering the next dialogue after the previous one has been completed
    private int _currentDialogueIndex = -2;
    private bool _inkValueUpToDate; // bool to help updating the ink values as they are not currently saved anywhere else; ducktape solution for now

    private string _questId;
    private bool _playerIsNear = false;
    private AudioSource _audioSource;
    private DialogueQuestNpCs _character = DialogueQuestNpCs.Bear;

    private void Start()
    {
        _questId = _questInfoForPoint.id;
        CheckDialogueProgressChanges(_questId);
        InitializeDialogueTracker();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += CheckDialogueProgressChanges;
        GameEventsManager.instance.DialogueEvents.OnStartDialogue += ToggleInteraction;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += ToggleInteraction;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest -= CheckDialogueProgressChanges;
        GameEventsManager.instance.DialogueEvents.OnStartDialogue -= ToggleInteraction;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue -= ToggleInteraction;
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear && _isInteractable)
        {
            InteractWithBear();
        }
    }

    private void InteractWithBear()
    {
        if (QuestManager.Instance.CheckQuestState(_questId) == QuestState.Finished)
        {
            return;
        }

        if (_dialogueFiles[_currentDialogueIndex] != null)
        {
            DialogueManager.Instance.StartDialogue(_dialogueFiles[_currentDialogueIndex]);
        }

        else
        {
            DialogueManager.Instance.StartDialogue(_dialogueBetweenQuests);
        }

        if (DialogueManager.Instance.IsDialoguePlaying)
        {
            AudioManager.Instance.PlaySound(_audioToPlayOnDialogueStarted, transform);
        }
    }

    private void CheckDialogueProgressChanges(string updatedQuestId)
    {
        if (updatedQuestId == _questId)
        {
            GameEventsManager.instance.DialogueEvents.RegisterPlayerNearNpc(_character, _playerIsNear);

            Invoke(nameof(UpdateDialogueProgressValues), 0.3f);
        }
    }

    private void UpdateDialogueProgressValues()
    {
        _currentDialogueIndex++;
    }

    private void InitializeDialogueTracker()
    {
        _currentDialogueIndex = QuestManager.Instance.GetQuestById(_questInfoForPoint.id).GetCurrentQuestStepIndex() - 1;
    }

    private void ToggleInteraction()
    {
        _isInteractable = !_isInteractable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = true;
            GameEventsManager.instance.DialogueEvents.RegisterPlayerNearNpc(_character, _playerIsNear);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = false;
            GameEventsManager.instance.DialogueEvents.RegisterPlayerNearNpc(_character, _playerIsNear);
        }
    }
}