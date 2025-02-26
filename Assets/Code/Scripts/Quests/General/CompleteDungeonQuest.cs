using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CompleteDungeonQuest : QuestStep
{
    [Header("Config")]
    [Tooltip("Set as 2 if the dungeon has both learning and boss area; set as 1 if there is only one stage")]
    [SerializeField] private int amountOfDungeonStages;
    [SerializeField] private int currentStageIndex;

    private int stagesCompleted;
    private string dungeonQuestId;

    private void Start()
    {
        dungeonQuestId = GetQuestId();
        Debug.Log(gameObject);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest += CompleteDungeon;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceDungeonQuest -= CompleteDungeon;
    }

    public void CompleteDungeon(string id)
    {
        Debug.Log("CompleteDungeon has been called.");
        if(id == dungeonQuestId)
        {
            stagesCompleted++;

            if (stagesCompleted < amountOfDungeonStages)
            {
                UpdateState();
            }

            else if (stagesCompleted >= amountOfDungeonStages)
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
        string state = amountOfDungeonStages.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        stagesCompleted = System.Int32.Parse(state);
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