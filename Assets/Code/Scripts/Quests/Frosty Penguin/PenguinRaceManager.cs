using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

        penguinDungeonEvents.onTimeRanOut += ShowCursor;
    }

    //private void OnEnable()
    //{
    //    GameEventsManager.instance.dialogueEvents.OnEndDialogue += TransitionToOverworld;
    //}

    private void OnDisable()
    {
        //GameEventsManager.instance.dialogueEvents.OnEndDialogue -= TransitionToOverworld;
        penguinDungeonEvents.onTimeRanOut -= ShowCursor;
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

        if (currentLap <= 2)
        {
            penguinDungeonEvents.LapFinished();
            lapCounterText.text = "Lap " + currentLap + "/2";
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
            AudioManager.Instance.PlaySound(AudioName.Action_PenguinRaceCompleted, transform);
            TriggerFinishDungeonDialogue();
        }
    }

    //private void TransitionToOverworld()
    //{
    //    if (dungeonQuestDialogue != null)
    //    {
    //        if (dungeonQuestDialogue.FinalDialogueCompleted())
    //        {
    //            StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.Scene.Overworld_VS, true);
    //            GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
    //        }
    //    }

    //    else
    //    {
    //        Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Penguin Race Manager. Please check inspector!");
    //    }
    //}

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

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public event Action onTimeRanOut;

    public void TimeRanOut()
    {
        if (onTimeRanOut != null)
        {
            onTimeRanOut();
        }
    }
}
