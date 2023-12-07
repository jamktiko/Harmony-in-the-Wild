using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerTriggerEnter : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Tick this if the collider should be disabled once player has passed it.")]
    [SerializeField] private bool disableTriggerAfterEvent;

    [SerializeField] private UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Debug.Log("Player triggered the event.");
            triggerEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Trigger") && disableTriggerAfterEvent)
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
