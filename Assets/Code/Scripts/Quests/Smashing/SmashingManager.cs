using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SmashingManager : MonoBehaviour
{
    [FormerlySerializedAs("questScriptableObject")]
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject _questScriptableObject;

    [FormerlySerializedAs("restartDialogue")]
    [Header("Needed References")]
    [SerializeField] private TextAsset _restartDialogue;
    [FormerlySerializedAs("questCanvas")] [SerializeField] private GameObject _questCanvas;
    [FormerlySerializedAs("attemptCounterText")] [SerializeField] private TextMeshProUGUI _attemptCounterText;
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;
    [FormerlySerializedAs("startPosition")] [SerializeField] private Transform _startPosition;

    public static SmashingManager Instance;

    private string _currentQuestId;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Manager.");
        }

        Instance = this;
    }

    private void Start()
    {
        _currentQuestId = _questScriptableObject.id;

        GameEventsManager.instance.QuestEvents.OnStartQuest += StartRockSmash;
        GameEventsManager.instance.QuestEvents.OnFinishQuest += FinishRockSmash;

        StartCoroutine(QuestProgressCheckDelay());
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest -= StartRockSmash;
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= FinishRockSmash;
    }

    private void StartRockSmash(string id)
    {
        if (_currentQuestId == id)
        {
            SmashingRockSpawner.Instance.SpawnStartingRocks();
            AbilityManager.Instance.UnlockAbility(Abilities.RockDestroying);
        }
    }

    public void RestartSmashing()
    {
        SmashingRockSpawner.Instance.ResetRocks();
        DialogueManager.Instance.StartDialogue(_restartDialogue);

        FoxMovement.Instance.gameObject.transform.position = _startPosition.position;
    }

    private void FinishRockSmash(string id)
    {
        if (_currentQuestId == id)
        {
            SmashingRockSpawner.Instance.DestroyRocks();
        }
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT
    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (QuestManager.Instance.CheckQuestState(_currentQuestId) == QuestState.InProgress)
        {
            StartRockSmash(_currentQuestId);
        }
    }

    public void ToggleQuestCanvasVisibility(bool visibility)
    {
        _questCanvas.SetActive(visibility);
    }

    public void UpdateAttemptCounter(int attemptsLeft, int maxAttempts)
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI("Smashing!", "Use the smashing ability to break rocks and retrieve the ore", "Attempts left " + attemptsLeft + "/" + maxAttempts);
    }
}
