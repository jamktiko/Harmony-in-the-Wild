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
    [SerializeField] private DungeonQuestDialogue dungeonQuestDialogue;
    [SerializeField] private PenguinTimer timer;

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

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += TransitionToOverworld;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= TransitionToOverworld;
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
            lapCounterText.text = "Lap " + currentLap + "/2";
            //GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Lap " + currentLap + "/2");
            AddLapObstacles();
        }

        else
        {
            CheckQuestCompletionRequirements();
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

    private void CheckQuestCompletionRequirements()
    {
        float time = timer.GetFinalTimeAsFloat();

        if (time < 180f)
        {
            penguinDungeonEvents.RaceFinished();
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questSO.id);
            TriggerFinishDungeonDialogue();
        }

        else
        {
            Debug.Log("Not fast enough, try again");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void TransitionToOverworld()
    {
        if (dungeonQuestDialogue != null)
        {
            if (dungeonQuestDialogue.FinalDialogueCompleted())
            {
                StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, "Overworld", true);
                SceneManager.LoadScene("Storybook");
            }
        }

        else
        {
            Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Penguin Race Manager. Please check inspector!");
        }
    }

    private void TriggerFinishDungeonDialogue()
    {
        if (dungeonQuestDialogue != null)
        {
            dungeonQuestDialogue.PlayFinishDungeonDialogue();
        }

        else
        {
            Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Penguin Race Manager. Please check inspector!");
        }
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
