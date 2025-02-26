using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SmashingAttemptCounter : QuestStep
{
    [FormerlySerializedAs("questScriptableObject")]
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject _questScriptableObject;

    [FormerlySerializedAs("maxAttempts")]
    [Header("Debug")]
    [SerializeField] private int _maxAttempts = 5;
    [FormerlySerializedAs("attemptsLeft")] [SerializeField] private int _attemptsLeft;

    public static SmashingAttemptCounter Instance;

    private string _currentQuestId;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Attempt Counter.");
        }

        Instance = this;
    }

    private void Start()
    {
        _currentQuestId = _questScriptableObject.id;

        StartCoroutine(QuestProgressCheckDelay());

        _attemptsLeft = _maxAttempts;

        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            SmashingManager.Instance.UpdateAttemptCounter(_attemptsLeft, _maxAttempts);
        }
    }

    public void UpdateProgress(bool oreWasFound)
    {
        if (oreWasFound)
        {
            Debug.Log("You found the ore!");
            FinishQuestStep();
        }

        else
        {
            _attemptsLeft--;

            //Debug.Log("Attempts for locating the ore left: " + attemptsLeft);

            if (_attemptsLeft <= 0)
            {
                Debug.Log("Restarting Smashing!...");
                SmashingManager.Instance.RestartSmashing();
                _attemptsLeft = _maxAttempts;
            }

            UpdateState();
            SmashingManager.Instance.UpdateAttemptCounter(_attemptsLeft, _maxAttempts);
        }
    }

    private void UpdateState()
    {
        string state = _attemptsLeft.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _attemptsLeft = System.Int32.Parse(state);

        UpdateState();
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT
    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            if (QuestManager.Instance.CheckQuestState(_currentQuestId) == QuestState.InProgress)
            {
                SmashingManager.Instance.ToggleQuestCanvasVisibility(true);
            }
        }
    }
}
