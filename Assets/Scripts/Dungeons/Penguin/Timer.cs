using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxTime;
    [SerializeField] private float timeDecreaseByLap;

    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Debug")]
    private bool isRaceInProgress;

    // private variables
    private float currentTime;
    private Coroutine timerCoroutine;

    private void Start()
    {
        currentTime = maxTime;

        var convertedTime = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Time " + string.Format("{0:00}:{1:00}", convertedTime.Minutes, convertedTime.Seconds);

        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetTimeAfterInterruptedLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += StartTimerForNewLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += StopTimer;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetTimeAfterInterruptedLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= StartTimerForNewLap;
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= StopTimer;
    }

    public void StartTimer()
    {
        isRaceInProgress = true;

        if(timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerProgress());
        }
    }

    private IEnumerator TimerProgress()
    {
        currentTime = maxTime;

        while (isRaceInProgress)
        {
            currentTime -= 1;

            var convertedTime = TimeSpan.FromSeconds(currentTime);
            timerText.text = "Time " + string.Format("{0:00}:{1:00}", convertedTime.Minutes, convertedTime.Seconds);

            if(currentTime >= maxTime)
            {
                Debug.Log("Time has run out!");
                isRaceInProgress = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void StartTimerForNewLap()
    {
        isRaceInProgress = false;
        StopCoroutine(timerCoroutine);

        maxTime -= timeDecreaseByLap;
        currentTime = maxTime;

        isRaceInProgress = true;
        timerCoroutine = StartCoroutine(TimerProgress());
    }

    private void ResetTimeAfterInterruptedLap()
    {
        isRaceInProgress = false;
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;

        currentTime = maxTime;

        var convertedTime = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Time " + string.Format("{0:00}:{1:00}", convertedTime.Minutes, convertedTime.Seconds);
    }

    private void StopTimer()
    {
        isRaceInProgress = false;
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }
}
