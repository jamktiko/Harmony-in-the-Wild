using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingManager : QuestStep
{
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject questScriptableObject;

    [Header("Needed References")]
    [SerializeField] private TextAsset restartDialogue;

    [Header("Debug")]
    [SerializeField] private bool oreFound;
    [SerializeField] private int maxAttempts = 5;
    [SerializeField] private int attemptsLeft;

    public static SmashingManager instance;

    private string currentQuestId;
    private GameObject questCanvas;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Manager.");
        }

        instance = this;
    }

    private void Start()
    {
        currentQuestId = questScriptableObject.id;
        attemptsLeft = maxAttempts;
        questCanvas = GameObject.FindGameObjectWithTag("QuestCanvas").transform.GetChild(0).gameObject;
        StartRockSmash();

        GameEventsManager.instance.questEvents.onFinishQuest += FinishRockSmash;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishRockSmash;
    }

    private void StartRockSmash()
    {
        SpawnNewRocks();
        questCanvas.SetActive(true);
    }

    public void SpawnNewRocks()
    {
        SmashingRockSpawner.instance.SpawnStartingRocks();
    }

    public void UpdateProgress(bool oreWasFound)
    {
        if (oreWasFound)
        {
            oreFound = true;
            FinishQuestStep();
        }

        else
        {
            attemptsLeft--;

            if(attemptsLeft <= 0)
            {
                SmashingRockSpawner.instance.ResetRocks();
                DialogueManager.instance.StartDialogue(restartDialogue);
                attemptsLeft = maxAttempts;
            }
        }
    }

    private void FinishRockSmash(string id)
    {
        if(currentQuestId == id)
        {
            SmashingRockSpawner.instance.DestroyRocks();
            questCanvas.SetActive(true);
        }
    }

    private void UpdateState()
    {
        string state = oreFound.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        oreFound = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
