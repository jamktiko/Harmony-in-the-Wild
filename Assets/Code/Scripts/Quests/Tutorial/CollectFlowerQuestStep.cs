using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectFlowerQuestStep : QuestStep
{
    public static CollectFlowerQuestStep instance;

    private bool collectedFlower;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Collect Flower Quest Steps in the scene!");
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

    public void CollectFlower()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = collectedFlower.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        collectedFlower = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
