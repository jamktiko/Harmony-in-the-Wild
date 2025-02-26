using UnityEngine.SceneManagement;

public class OpenMapQuestStep : QuestStep
{
    private int _mapActionsDone;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
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
            GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
        }
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.OpenMapInput.WasPressedThisFrame())
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        _mapActionsDone++;

        // if map has been both opened and closed, progress in the quest
        if (_mapActionsDone >= 2)
        {
            FinishQuestStep();
        }

        // otherwise just update the quest state
        else
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        string state = _mapActionsDone.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _mapActionsDone = System.Int32.Parse(state);

        UpdateState();
    }

}
