using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PenguinRaceManager : MonoBehaviour
{
    public static PenguinRaceManager instance { get; private set; }

    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI lapCounterText;
    [SerializeField] private GameObject alertView;
    [SerializeField] private GameObject winView;

    [Header("Storybook Config")]
    [SerializeField] private int storybookSectionIndex;

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

    // ---------------------
    // TRIGGER CUSTOM EVENTS
    // ---------------------

    public void LapInterrupted()
    {
        penguinDungeonEvents.LapInterrupted();
    }

    public void LapFinished()
    {
        currentLap++;

        Debug.Log("Current lap is " + currentLap);

        if(currentLap <= 3)
        {
            penguinDungeonEvents.LapFinished();
            lapCounterText.text = currentLap + "/3";
        }

        else
        {
            penguinDungeonEvents.RaceFinished();
            winView.SetActive(true);
            StartCoroutine(TransitionToOverworld());
        }

    }

    // -----------------------------------------------------
    // PROGRESS-RELATED METHODS (can be called from anywhere)
    // -----------------------------------------------------

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

    private IEnumerator TransitionToOverworld()
    {
        yield return new WaitForSeconds(3f);

        StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, "Overworld", true);
        SceneManager.LoadScene("Storybook");
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

    public event Action onRaceFinished;

    public void RaceFinished()
    {
        if (onRaceFinished != null)
        {
            onRaceFinished();
        }
    }
}
