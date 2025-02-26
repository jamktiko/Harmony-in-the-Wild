using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class RaceProgressCollider : MonoBehaviour
{
    [FormerlySerializedAs("isStartLine")]
    [Header("Tracker Type")]
    [SerializeField] private bool _isStartLine;
    [FormerlySerializedAs("isFinishLine")] [SerializeField] private bool _isFinishLine;

    [FormerlySerializedAs("triggeredRaceEvent")]
    [Header("Possible Event")]
    [SerializeField] private UnityEvent _triggeredRaceEvent;

    [FormerlySerializedAs("SoundTrackStart")]
    [Header("Audio")]
    [SerializeField] private AudioSource _soundTrackStart;

    // private variables
    private bool _canTriggerEvents = true;

    private void Start()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted += LapInterruptionReset;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += LapFinishReset;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted -= LapInterruptionReset;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= LapFinishReset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            // trigger the event, if this is either start or finish line
            if (_isFinishLine || _isStartLine)
            {
                if (_triggeredRaceEvent != null && _canTriggerEvents)
                {
                    _triggeredRaceEvent.Invoke();
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
            if (_isStartLine)
            {
                GetComponent<Collider>().isTrigger = false;
            }

            // if this is a waypoint for progress tracking, enable invisible wall after player has passed the trigger
            else if (!_isFinishLine)
            {
                transform.GetChild(0).gameObject.SetActive(true);

                if (_triggeredRaceEvent != null)
                {
                    _triggeredRaceEvent.Invoke();
                    StartCoroutine(DelayBeforeNextTrigger());
                }
            }
        }
    }

    private void LapFinishReset()
    {
        if (_isStartLine)
        {
            return;
        }

        else if (!_isFinishLine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void LapInterruptionReset()
    {
        if (_isStartLine)
        {
            GetComponent<Collider>().isTrigger = true;
        }

        else if (!_isFinishLine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void StartAudio()
    {
        if (!_soundTrackStart.isPlaying && _isStartLine) { _soundTrackStart.Play(); }
    }

    private IEnumerator DelayBeforeNextTrigger()
    {
        _canTriggerEvents = false;

        yield return new WaitForSeconds(1);

        _canTriggerEvents = true;
    }
}
