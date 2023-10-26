using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PenguinRaceManager : MonoBehaviour
{
    public static PenguinRaceManager instance { get; private set; }

    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI lapCounterText;
    [SerializeField] private GameObject alertView;

    [Header("Debug")]
    [SerializeField] private int currentLap = 1;

    public PenguinDungeonEvents penguinDungeonEvents;

    private void Awake()
    {
        // create singleton
        if (instance != null)
        {
            Debug.LogError("There is more than one Penguin Race Manager in the scene");
        }

        instance = this;

        penguinDungeonEvents = new PenguinDungeonEvents();
    }

    public void LapInterrupted()
    {
        penguinDungeonEvents.LapInterrupted();
    }

    public void LapFinished()
    {
        penguinDungeonEvents.LapFinished();
        currentLap++;

        if(currentLap >= 3)
        {
            lapCounterText.text = currentLap + "/3";
        }

    }

    public void WrongWay()
    {
        if (!alertView.activeInHierarchy)
        {
            alertView.SetActive(true);
            StartCoroutine(HideAlertView());
        }
    }

    private IEnumerator HideAlertView()
    {
        yield return new WaitForSeconds(1.5f);

        alertView.SetActive(false);
    }

    public void ToggleFinishLineColliders(Collider finishline)
    {
        finishline.isTrigger = !finishline.isTrigger;
    }
}

public class PenguinDungeonEvents
{
    public event Action onLapInterrupted;

    public void LapInterrupted()
    {
        if (onLapInterrupted != null)
        {
            onLapInterrupted();
        }
    }

    public event Action onLapFinished;

    public void LapFinished()
    {
        if (onLapFinished != null)
        {
            onLapFinished();
        }
    }
}
