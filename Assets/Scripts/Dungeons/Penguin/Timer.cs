using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxTime;

    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Debug")]
    private bool raceInProgress;

    // private variables
    private float currentTime;

    private void Start()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetTimeAfterInterruptedLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += StartTimerForNewLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += StopTimer;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetTimeAfterInterruptedLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= StartTimer;
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= StopTimer;
    }

    public void StartTimer()
    {
        raceInProgress = true;

        StartCoroutine(TimerProgress());
    }

    private IEnumerator TimerProgress()
    {
        while (raceInProgress)
        {
            currentTime += 1;

            var convertedTime = TimeSpan.FromSeconds(currentTime);
            timerText.text = string.Format("{0:00}:{1:00}", convertedTime.Minutes, convertedTime.Seconds);

            if(currentTime >= maxTime)
            {
                Debug.Log("Time has run out!");
                raceInProgress = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void StartTimerForNewLap()
    {
        raceInProgress = false;

        currentTime = 0;

        raceInProgress = true;
        StartCoroutine(TimerProgress());
    }

    private void ResetTimeAfterInterruptedLap()
    {
        raceInProgress = false;

        currentTime = 0;
    }

    private void StopTimer()
    {
        raceInProgress = false;
    }
}
