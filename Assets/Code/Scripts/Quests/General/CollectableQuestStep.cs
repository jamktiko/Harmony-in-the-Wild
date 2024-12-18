using UnityEngine.SceneManagement;

public class CollectableQuestStep : QuestStep
{
    public int itemsCollected = 0;
    private int itemToComplete = 5;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress + " " + itemsCollected + "/" + itemToComplete);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetUIInOverworld;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetUIInOverworld;
    }

    private void SetUIInOverworld(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }
    }

    public void CollectableProgress()
    {
        itemsCollected++;
        UpdateState();
        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI(progress + " " + itemsCollected + "/" + itemToComplete);

        if (itemsCollected >= itemToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = itemsCollected.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        itemsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
