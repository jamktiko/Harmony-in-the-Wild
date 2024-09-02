using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PenguinRaceManager : MonoBehaviour
{
    public static PenguinRaceManager instance { get; private set; }

    [Header("Quest SO")]
    [SerializeField] private QuestScriptableObject questSO;

    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI lapCounterText;
    [SerializeField] private GameObject alertView;
    [SerializeField] private GameObject lap1_Obstacles;
    [SerializeField] private GameObject lap2_Obstacles;

    [Header("Storybook Config")]
    [SerializeField] private int storybookSectionIndex;

    private int currentLap = 1;

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

        if(currentLap <= 2)
        {
            penguinDungeonEvents.LapFinished();
            //GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Lap " + currentLap + "/2");
            AddLapObstacles();
        }

        else
        {
            penguinDungeonEvents.RaceFinished();
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questSO.id);
            StartCoroutine(TransitionToOverworld());
        }

    }

    // -----------------------------------------------------
    // PROGRESS-RELATED METHODS (can be called from anywhere)
    // -----------------------------------------------------

    public void AddLapObstacles()
    {
        lap2_Obstacles.SetActive(true);
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

    private IEnumerator TransitionToOverworld()
    {
        yield return new WaitForSeconds(3f);

        StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, "OverWorld - VS", true);
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
