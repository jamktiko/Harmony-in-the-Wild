using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class OnPlayerTriggerEnter : MonoBehaviour
{
    [FormerlySerializedAs("disableTriggerAfterEvent")]
    [Header("Config")]
    [Tooltip("Tick this if the collider should be disabled once player has passed it.")]
    [SerializeField] private bool _disableTriggerAfterEvent;

    [FormerlySerializedAs("triggerEvent")] [SerializeField] private UnityEvent _triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Debug.Log("Player triggered the event.");
            _triggerEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && _disableTriggerAfterEvent)
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
