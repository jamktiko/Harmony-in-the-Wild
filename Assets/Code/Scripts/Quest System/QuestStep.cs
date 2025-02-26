using UnityEngine;
using UnityEngine.Serialization;

public abstract class QuestStep : MonoBehaviour
{
    private bool _isFinished = false;

    private string _questId;

    private int _stepIndex;

    [FormerlySerializedAs("positionInScene")]
    [Header("Fill only if quest requires a waypoint")]

    [SerializeField] public Vector3 PositionInScene;

    [FormerlySerializedAs("hasWaypoint")] [SerializeField] public bool HasWaypoint;

    //public string stepName;

    [FormerlySerializedAs("objective")] [Header("Fill for Side Quests Only")]

    public string Objective;

    [FormerlySerializedAs("progress")] [Tooltip("Set in the form as 'Apples collected '; rest is autofilled through script")]

    public string Progress;

    public void InitializeQuestStep(string id, int stepIndex, string questStepState)
    {
        _questId = id;
        this._stepIndex = stepIndex;


        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }

        else if (id == "Whale Diet")
        {
            SetQuestStepState("0");
        }
        SaveManager.Instance.SaveGame();
    }

    protected string GetQuestId()
    {
        return _questId;
    }

    protected void FinishQuestStep()
    {
        if (!_isFinished)
        {
            _isFinished = true;

            GameEventsManager.instance.QuestEvents.AdvanceQuest(_questId);
            Debug.Log("Finished quest step: " + _questId);
            Invoke("DestroyObject", 0);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.instance.QuestEvents.QuestStepStateChange(_questId, _stepIndex, new QuestStepState(newState));
        SaveManager.Instance.SaveGame();
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected abstract void SetQuestStepState(string state);
}
