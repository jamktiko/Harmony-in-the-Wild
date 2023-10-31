using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingAttemptCounter : QuestStep
{
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject questScriptableObject;

    [Header("Debug")]
    [SerializeField] private bool oreFound;
    [SerializeField] private int maxAttempts = 5;
    [SerializeField] private int attemptsLeft;

    public static SmashingAttemptCounter instance;

    private string currentQuestId;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Attempt Counter.");
        }

        instance = this;
    }

    private void Start()
    {
        currentQuestId = questScriptableObject.id;

        StartCoroutine(QuestProgressCheckDelay());

        attemptsLeft = maxAttempts;
        SmashingManager.instance.UpdateAttemptCounter(attemptsLeft, maxAttempts);
    }

    public void UpdateProgress(bool oreWasFound)
    {
        if (oreWasFound)
        {
            Debug.Log("You found the ore!");
            oreFound = true;
            FinishQuestStep();
        }

        else
        {
            attemptsLeft--;

            Debug.Log("Attempts for locating the ore left: " + attemptsLeft);

            if (attemptsLeft <= 0)
            {
                Debug.Log("Restarting Smashing!...");
                SmashingManager.instance.RestartSmashing();
                attemptsLeft = maxAttempts;
            }

            UpdateState();
            SmashingManager.instance.UpdateAttemptCounter(attemptsLeft, maxAttempts);
        }
    }

    private void UpdateState()
    {
        string state = attemptsLeft.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        attemptsLeft = System.Int32.Parse(state);

        UpdateState();
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT
    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (QuestManager.instance.CheckQuestState(currentQuestId) == QuestState.IN_PROGRESS)
        {
            SmashingManager.instance.ToggleQuestCanvasVisibility(true);
        }
    }
}
