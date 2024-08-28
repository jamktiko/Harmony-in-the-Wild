using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PenguinResultsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI trophyText;
    [SerializeField] private Image trophyImage;
    [SerializeField] private PenguinTimer timer;

    [Header("Trophy Config")]
    [SerializeField] private float highestTrophy = 120f;
    [SerializeField] private float mediumTrophy = 180f;
    [SerializeField] private float lowestTrophy = 240f;
    [SerializeField] private List<Sprite> trophyImages;

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

        if(time <= highestTrophy)
        {
            trophyResults = "GOLDEN TROPHY";
            trophyImage.sprite = trophyImages[0];
            trophyImage.enabled = true;
        }

        else if(time > highestTrophy && time <= mediumTrophy)
        {
            trophyResults = "SILVER TROPHY";
            trophyImage.sprite = trophyImages[1];
            trophyImage.enabled = true;
        }

        else if(time > mediumTrophy && time <= lowestTrophy)
        {
            trophyResults = "BRONZE TROPHY";
            trophyImage.sprite = trophyImages[2];
            trophyImage.enabled = true;
        }

        else
        {
            trophyResults = "no trophies";
            trophyImage.enabled = false;
        }

        return trophyResults;
    }
}
