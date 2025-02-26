using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PenguinRaceManager : MonoBehaviour
{
    public static PenguinRaceManager instance { get; private set; }

    [FormerlySerializedAs("questSO")]
    [Header("Quest SO")]
    [SerializeField] private QuestScriptableObject _questSo;

    [FormerlySerializedAs("lapCounterText")]
    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI _lapCounterText;
    [FormerlySerializedAs("alertView")] [SerializeField] private GameObject _alertView;
    [FormerlySerializedAs("lap1_Obstacles")] [SerializeField] private GameObject _lap1Obstacles;
    [FormerlySerializedAs("lap2_Obstacles")] [SerializeField] private GameObject _lap2Obstacles;
    [FormerlySerializedAs("dungeonQuestDialogue")] [SerializeField] private DungeonQuestDialogue _dungeonQuestDialogue;
    [FormerlySerializedAs("timer")] [SerializeField] private PenguinTimer _timer;

    private int _currentLap = 1;

    public PenguinDungeonEvents PenguinDungeonEvents;

    private void Awake()
    {
        // create singleton
        if (instance != null)
        {
            Debug.LogError("There is more than one Penguin Race Manager in the scene");
        }

        instance = this;

        PenguinDungeonEvents = new PenguinDungeonEvents();

        PenguinDungeonEvents.OnTimeRanOut += ShowCursor;
    }

    //private void OnEnable()
    //{
    //    GameEventsManager.instance.dialogueEvents.OnEndDialogue += TransitionToOverworld;
    //}

    private void OnDisable()
    {
        //GameEventsManager.instance.dialogueEvents.OnEndDialogue -= TransitionToOverworld;
        PenguinDungeonEvents.OnTimeRanOut -= ShowCursor;
    }

    // ---------------------
    // TRIGGER CUSTOM EVENTS
    // ---------------------

    public void LapInterrupted()
    {
        PenguinDungeonEvents.LapInterrupted();
    }

    public void LapFinished()
    {
        _currentLap++;

        if (_currentLap <= 2)
        {
            PenguinDungeonEvents.LapFinished();
            _lapCounterText.text = "Lap " + _currentLap + "/2";
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
        _lap2Obstacles.SetActive(true);
    }

    public void WrongWay()
    {
        if (!_alertView.activeInHierarchy)
        {
            _alertView.SetActive(true);
            StartCoroutine(HideAlertView());
        }
    }

    private IEnumerator HideAlertView()
    {
        yield return new WaitForSeconds(1.5f);

        _alertView.SetActive(false);
    }

    private void CheckQuestCompletionRequirements()
    {
        float time = _timer.GetFinalTimeAsFloat();

        if (time < 180f)
        {
            PenguinDungeonEvents.RaceFinished();
            GameEventsManager.instance.QuestEvents.AdvanceDungeonQuest(_questSo.id);
            AudioManager.Instance.PlaySound(AudioName.ActionPenguinRaceCompleted, transform);
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
        if (_dungeonQuestDialogue != null)
        {
            _dungeonQuestDialogue.PlayFinishDungeonDialogue();
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
    public event Action OnLapInterrupted;

    public void LapInterrupted()
    {
        if (OnLapInterrupted != null)
        {
            OnLapInterrupted();
        }
    }

    public event Action OnLapFinished;

    public void LapFinished()
    {
        if (OnLapFinished != null)
        {
            OnLapFinished();
        }
    }

    public event Action OnRaceFinished;

    public void RaceFinished()
    {
        if (OnRaceFinished != null)
        {
            OnRaceFinished();
        }
    }

    public event Action OnTimeRanOut;

    public void TimeRanOut()
    {
        if (OnTimeRanOut != null)
        {
            OnTimeRanOut();
        }
    }
}
