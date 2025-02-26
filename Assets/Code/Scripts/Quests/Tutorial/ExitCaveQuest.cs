using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCaveQuest : QuestStep
{
    public static ExitCaveQuest instance;

    private bool hasExited;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Exit Cave Quests in the scene!");
        }

        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
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
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }
    }

    public void ExitCave()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = hasExited.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        hasExited = System.Convert.ToBoolean(state);

        UpdateState();
    }

}
