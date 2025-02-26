using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectFlowerQuestStep : QuestStep
{
    public static CollectFlowerQuestStep Instance;

    private bool _collectedFlower;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one Collect Flower Quest Steps in the scene!");
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

    public void CollectFlower()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = _collectedFlower.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _collectedFlower = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
