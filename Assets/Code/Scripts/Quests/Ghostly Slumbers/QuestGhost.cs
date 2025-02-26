using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestGhost : MonoBehaviour
{
    [FormerlySerializedAs("relativeIndex")]
    [Header("Config")]
    [SerializeField] private int _relativeIndex;

    [FormerlySerializedAs("initialDialogue")]
    [Header("Ghost Dialogue")]
    [SerializeField] private TextAsset _initialDialogue;
    [FormerlySerializedAs("secondaryDialogueOptions")] [SerializeField] private List<TextAsset> _secondaryDialogueOptions;

    private bool _playerIsNear;
    private bool _beenSpokenTo;

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame())
        {
            InteractWithGhost();
        }
    }

    public void InteractWithGhost()
    {
        if (_playerIsNear && !DialogueManager.Instance.IsDialoguePlaying)
        {
            switch (_beenSpokenTo)
            {
                // first dialogue with the ghost
                case false:
                    _beenSpokenTo = true;
                    DialogueManager.Instance.StartDialogue(_initialDialogue);
                    GhostlySlumbersManager.Instance.TalkedToRelative(_relativeIndex);
                    break;

                // interacting with the ghost again after the initial dialogue
                case true:
                    DialogueManager.Instance.StartDialogue(_secondaryDialogueOptions[Random.Range(0, _secondaryDialogueOptions.Count)]);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;
        }
    }
}
