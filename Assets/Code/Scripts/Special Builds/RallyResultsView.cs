using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RallyResultsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI trophyText;
    [SerializeField] private RallyTimer timer;

    [Header("Trophy Config")]
    [SerializeField] private float highestTrophy;
    [SerializeField] private float mediumTrophy;
    [SerializeField] private float lowestTrophy;

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += ShowView;
    }

    private void ShowView()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        // set timer text
        timerText.text = timer.GetFinalTime();

        // set trophy

    }
}
