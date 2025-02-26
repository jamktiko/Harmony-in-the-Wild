using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcDialogue : MonoBehaviour
{
    [FormerlySerializedAs("questDialoguePairs")] [SerializeField] private List<NpcQuestDialoguePair> _questDialoguePairs;
    [FormerlySerializedAs("defaultDialogue")] [SerializeField] private TextAsset _defaultDialogue;
    [FormerlySerializedAs("audioToPlayOnDialogueStarted")] [SerializeField] private AudioName _audioToPlayOnDialogueStarted;

    private List<TextAsset> _possibleDialogues = new List<TextAsset>();
    private bool _playerIsNear;

    private void Start()
    {
        Invoke(nameof(CreateListOfPossibleDialogues), 1f);
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear)
        {
            SetDialogueToPlay();
        }
    }

    private void SetDialogueToPlay()
    {
        // fetch a random dialogue from the list of possible dialogues
        int dialogueIndex = Random.Range(0, _possibleDialogues.Count);

        Debug.Log("Ready to start a NPC dialogue.");

        DialogueManager.Instance.StartDialogue(_possibleDialogues[dialogueIndex]);

        if (DialogueManager.Instance.IsDialoguePlaying)
        {
            AudioManager.Instance.PlaySound(_audioToPlayOnDialogueStarted, transform);
        }
    }

    private void CreateListOfPossibleDialogues()
    {
        // go through the main quests and add the possible dialogues to the list
        foreach (NpcQuestDialoguePair questDialoguePair in _questDialoguePairs)
        {
            QuestState state = QuestManager.Instance.CheckQuestState(questDialoguePair.MainQuest.id);

            if (state == QuestState.Finished)
            {
                _possibleDialogues.Add(questDialoguePair.DialogueOption);
            }
        }

        // if none of the main quests have been completed yet, add the default dialogue as an option
        if (_possibleDialogues.Count == 0)
        {
            _possibleDialogues.Add(_defaultDialogue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = false;
        }
    }
}
