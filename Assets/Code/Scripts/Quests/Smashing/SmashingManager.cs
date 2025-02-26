using System.Collections;
using TMPro;
using UnityEngine;

public class SmashingManager : MonoBehaviour
{
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject questScriptableObject;

    [Header("Needed References")]
    [SerializeField] private TextAsset restartDialogue;
    [SerializeField] private GameObject questCanvas;
    [SerializeField] private TextMeshProUGUI attemptCounterText;
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPosition;

    public static SmashingManager instance;

    private string currentQuestId;

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

        GameEventsManager.instance.questEvents.OnStartQuest += StartRockSmash;
        GameEventsManager.instance.questEvents.OnFinishQuest += FinishRockSmash;

        StartCoroutine(QuestProgressCheckDelay());
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest -= StartRockSmash;
        GameEventsManager.instance.questEvents.OnFinishQuest -= FinishRockSmash;
    }

    private void StartRockSmash(string id)
    {
        if (currentQuestId == id)
        {
            SmashingRockSpawner.instance.SpawnStartingRocks();
            AbilityManager.Instance.UnlockAbility(Abilities.RockDestroying);
        }
    }

    public void RestartSmashing()
    {
        SmashingRockSpawner.instance.ResetRocks();
        DialogueManager.instance.StartDialogue(restartDialogue);

        FoxMovement.instance.gameObject.transform.position = startPosition.position;
    }

    private void FinishRockSmash(string id)
    {
        if (currentQuestId == id)
        {
            SmashingRockSpawner.instance.DestroyRocks();
        }
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT
    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (QuestManager.instance.CheckQuestState(currentQuestId) == QuestState.IN_PROGRESS)
        {
            StartRockSmash(currentQuestId);
        }
    }

    public void ToggleQuestCanvasVisibility(bool visibility)
    {
        questCanvas.SetActive(visibility);
    }

    public void UpdateAttemptCounter(int attemptsLeft, int maxAttempts)
    {
        GameEventsManager.instance.questEvents.ShowQuestUI("Smashing!", "Use the smashing ability to break rocks and retrieve the ore", "Attempts left " + attemptsLeft + "/" + maxAttempts);
    }
}
