using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwimmingColliderDialogue : MonoBehaviour
{
    [FormerlySerializedAs("possibleDialogues")] [SerializeField] private List<TextAsset> _possibleDialogues;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            Debug.Log("Player detected.");
            DialogueManager.Instance.StartDialogue(_possibleDialogues[Random.Range(0, _possibleDialogues.Count)]);
        }
    }
}
