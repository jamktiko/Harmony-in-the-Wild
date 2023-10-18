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
}
