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
    [SerializeField] private float highestTrophy = 120f;
    [SerializeField] private float mediumTrophy = 180f;
    [SerializeField] private float lowestTrophy = 240f;

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += ShowView;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= ShowView;
    }

    private void ShowView()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        // set timer text
        timerText.text = timer.GetFinalTimeAsString();

        // set trophy
        trophyText.text = SetTrophyText();
    }

    private string SetTrophyText()
    {
        string trophyResults = "";

        float time = timer.GetFinalTimeAsFloat();
        Debug.Log("Final time value:" + time);

        if(time <= highestTrophy)
        {
            trophyResults = "GOLDEN TROPHY";
        }

        else if(time >= lowestTrophy)
        {
            trophyResults = "BRONZE TROPHY";
        }

        else
        {
            trophyResults = "SILVER TROPHY";
        }

        return trophyResults;
    }
}
