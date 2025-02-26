using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCaveQuest : QuestStep
{
    public static ExitCaveQuest Instance;

    private bool _hasExited;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one Exit Cave Quests in the scene!");
        }

        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetUIInTutorial;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetUIInTutorial;
    }

    private void SetUIInTutorial(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Tutorial", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
        }
    }

    public void ExitCave()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = _hasExited.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _hasExited = System.Convert.ToBoolean(state);

        UpdateState();
    }

}
