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

    [Header("Audio")]
    [SerializeField] private AudioSource SoundTrackStart;

    // private variables
    private bool canTriggerEvents = true;

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
        if (other.gameObject.CompareTag("Trigger"))
        {
            // trigger the event, if this is either start or finish line
            if (isFinishLine || isStartLine)
            {
                if (triggeredRaceEvent != null && canTriggerEvents)
                {
                    triggeredRaceEvent.Invoke();
                    StartCoroutine(DelayBeforeNextTrigger());
                }
            }

            // show wrong way indicator if player hits the active invisible wall
            else if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                PenguinRaceManager.instance.WrongWay();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
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
                    StartCoroutine(DelayBeforeNextTrigger());
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
    public void StartAudio()
    {
        if (!SoundTrackStart.isPlaying&&isStartLine) { SoundTrackStart.Play(); }
    }

    private IEnumerator DelayBeforeNextTrigger()
    {
        canTriggerEvents = false;
        
        yield return new WaitForSeconds(1);

        canTriggerEvents = true;
    }
}
