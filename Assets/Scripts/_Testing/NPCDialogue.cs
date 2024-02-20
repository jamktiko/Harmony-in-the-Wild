using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueToPlay;

    private bool playerIsNear;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {
            DialogueManager.instance.StartDialogue(dialogueToPlay);
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
