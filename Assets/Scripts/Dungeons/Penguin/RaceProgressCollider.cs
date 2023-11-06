using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceProgressCollider : MonoBehaviour
{
    [Header("Tracker Type")]
    [SerializeField] private bool isStartLine;
    [SerializeField] private bool isFinishLine;

    [Header("Possible Event")]
    [SerializeField] private UnityEvent triggeredRaceEvent;

    private void Start()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += LapInterruptionReset;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += LapFinishReset;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= LapInterruptionReset;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= LapFinishReset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // trigger the event, if this is either start or finish line
            if (isFinishLine || isStartLine)
            {
                if (triggeredRaceEvent != null)
                {
                    triggeredRaceEvent.Invoke();
                }
            }

            else if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                PenguinRaceManager.instance.WrongWay();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            // if this is a start line, disable trigger
            if(isStartLine)
            {
                GetComponent<Collider>().isTrigger = false;
            }

            // if this is a waypoint for progress tracking, enable invisible wall after player has passed the trigger
            else if(!isFinishLine)
            {
                transform.GetChild(0).gameObject.SetActive(true);

                if (triggeredRaceEvent != null)
                {
                    triggeredRaceEvent.Invoke();
                }
            }
        }
    }

    private void LapFinishReset()
    {
        if (isStartLine)
        {
            return;
        }

        else if(!isFinishLine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void LapInterruptionReset()
    {
        if (isStartLine)
        {
            GetComponent<Collider>().isTrigger = true;
        }

        else if(!isFinishLine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
