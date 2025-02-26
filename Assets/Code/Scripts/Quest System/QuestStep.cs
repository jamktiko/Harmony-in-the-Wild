using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    private string questId;

    private int stepIndex;

    [Header("Fill only if quest requires a waypoint")]

    [SerializeField] public Vector3 positionInScene;

    [SerializeField] public bool hasWaypoint;

    //public string stepName;

    [Header("Fill for Side Quests Only")]

    public string objective;

    [Tooltip("Set in the form as 'Apples collected '; rest is autofilled through script")]

    public string progress;

    public void InitializeQuestStep(string id, int stepIndex, string questStepState)
    {
        questId = id;
        this.stepIndex = stepIndex;


        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }

        else if (id == "Whale Diet")
        {
            SetQuestStepState("0");
        }
        SaveManager.instance.SaveGame();
    }

    protected string GetQuestId()
    {
        return questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            GameEventsManager.instance.questEvents.AdvanceQuest(questId);
            Debug.Log("Finished quest step: " + questId);
            Invoke("DestroyObject", 0);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
        SaveManager.instance.SaveGame();
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected abstract void SetQuestStepState(string state);
}
