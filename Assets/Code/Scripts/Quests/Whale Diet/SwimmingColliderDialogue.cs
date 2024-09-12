using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingColliderDialogue : MonoBehaviour
{
    [SerializeField] private List<TextAsset> possibleDialogues;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            Debug.Log("Player detected.");
            DialogueManager.instance.StartDialogue(possibleDialogues[Random.Range(0, possibleDialogues.Count)]);
        }
    }
}
