using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class RallyTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private bool raceInProgress;
    private float currentTime;

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += StopTimer;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= StopTimer;
    }

    private void Update()
    {
        if (raceInProgress)
        {
            currentTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    public void ToggleTimer(bool timerOn)
    {
        transform.GetChild(0).gameObject.SetActive(timerOn);
        raceInProgress = timerOn;
    }

    private void StopTimer()
    {
        ToggleTimer(false);
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt(currentTime * 100 % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public string GetFinalTimeAsString()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt(currentTime * 100 % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public float GetFinalTimeAsFloat()
    {
        return currentTime;
    }
}