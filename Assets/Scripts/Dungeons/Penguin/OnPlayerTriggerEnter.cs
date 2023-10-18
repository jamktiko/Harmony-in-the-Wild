using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerTriggerEnter : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player triggered the event.");
            triggerEvent.Invoke();
        }
    }
}
