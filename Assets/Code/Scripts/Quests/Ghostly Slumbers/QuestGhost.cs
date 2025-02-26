using System.Collections.Generic;
using UnityEngine;

public class QuestGhost : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int relativeIndex;

    [Header("Ghost Dialogue")]
    [SerializeField] private TextAsset initialDialogue;
    [SerializeField] private List<TextAsset> secondaryDialogueOptions;

    private bool playerIsNear;
    private bool beenSpokenTo;

    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame())
        {
            InteractWithGhost();
        }
    }

    public void InteractWithGhost()
    {
        if (playerIsNear && !DialogueManager.instance.isDialoguePlaying)
        {
            switch (beenSpokenTo)
            {
                // first dialogue with the ghost
                case false:
                    beenSpokenTo = true;
                    DialogueManager.instance.StartDialogue(initialDialogue);
                    GhostlySlumbersManager.instance.TalkedToRelative(relativeIndex);
                    break;

                // interacting with the ghost again after the initial dialogue
                case true:
                    DialogueManager.instance.StartDialogue(secondaryDialogueOptions[Random.Range(0, secondaryDialogueOptions.Count)]);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
