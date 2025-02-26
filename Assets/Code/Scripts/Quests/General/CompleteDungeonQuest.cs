using UnityEngine;
using UnityEngine.Serialization;


public class CompleteDungeonQuest : QuestStep
{
    [FormerlySerializedAs("amountOfDungeonStages")]
    [Header("Config")]
    [Tooltip("Set as 2 if the dungeon has both learning and boss area; set as 1 if there is only one stage")]
    [SerializeField] private int _amountOfDungeonStages;
    [FormerlySerializedAs("currentStageIndex")] [SerializeField] private int _currentStageIndex;

    private int _stagesCompleted;
    private string _dungeonQuestId;

    private void Start()
    {
        _dungeonQuestId = GetQuestId();
        Debug.Log(gameObject);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest += CompleteDungeon;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceDungeonQuest -= CompleteDungeon;
    }

    public void CompleteDungeon(string id)
    {
        Debug.Log("CompleteDungeon has been called.");
        if (id == _dungeonQuestId)
        {
            _stagesCompleted++;

            if (_stagesCompleted < _amountOfDungeonStages)
            {
                UpdateState();
            }

            else if (_stagesCompleted >= _amountOfDungeonStages)
            {
                Debug.Log("Sent request to finish quest step.");
                FinishQuestStep();
                //QuestManager.instance.RequestFinishQuest(id);
                //GameEventsManager.instance.questEvents.FinishQuest(id);
            }
        }
    }

    private void UpdateState()
    {
        string state = _amountOfDungeonStages.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _stagesCompleted = System.Int32.Parse(state);
        UpdateState();
    }
}

//[Header("Config")]
//[Tooltip("Set as 2 if the dungeon has both learning and boss area; set as 1 if there is only one stage")]
//[SerializeField] private int amountOfDungeonStages;

//private int stagesCompleted;
//private string dungeonQuestId;

//private void Start()
//{
//    dungeonQuestId = GetQuestId();
//}

//private void OnEnable()
//{
//    GameEventsManager.instance.questEvents.onAdvanceDungeonQuest += CompleteDungeon;
//}

//private void OnDisable()
//{
//    GameEventsManager.instance.questEvents.onAdvanceDungeonQuest -= CompleteDungeon;
//}

//public void CompleteDungeon(string id)
//{
//    if (id == dungeonQuestId)
//    {
//        stagesCompleted++;

//        if (stagesCompleted < amountOfDungeonStages)
//        {
//            UpdateState();
//        }

//        else if (stagesCompleted >= amountOfDungeonStages)
//        {
//            FinishQuestStep();
//        }
//    }
//}

//private void UpdateState()
//{
//    string state = amountOfDungeonStages.ToString();
//    ChangeState(state);
//}

//protected override void SetQuestStepState(string state)
//{
//    stagesCompleted = System.Int32.Parse(state);
//    UpdateState();
//}