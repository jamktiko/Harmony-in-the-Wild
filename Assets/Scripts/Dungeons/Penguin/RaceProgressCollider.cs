using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceProgressCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent triggeredRaceEvent;

    private void Start()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetCollider;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ResetCollider;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetCollider;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ResetCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && transform.GetChild(0).gameObject.activeInHierarchy)
        {
            PenguinRaceManager.instance.WrongWay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if(triggeredRaceEvent != null)
            {
                triggeredRaceEvent.Invoke();
            }
        }
    }

    private void ResetCollider()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
