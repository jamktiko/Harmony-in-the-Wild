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

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += StartTimerForNewLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += StopTimer;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= StartTimerForNewLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= StopTimer;
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

    private void StopTimer()
    {
        raceInProgress = false;
    }
}
