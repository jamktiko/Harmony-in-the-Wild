using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateQuestUIWithTrigger : MonoBehaviour
{
    [SerializeField] private string updatedQuestProgressUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            GameEventsManager.instance.questEvents.UpdateQuestProgressInUI(updatedQuestProgressUI);
            Destroy(this);
        }
    }
}
