using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    public static QuestManager instance;

    private int CurrentPlayerLevel;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Quest Manager.");
        }

        instance = this;

        // initialize quest map
        questMap = CreateQuestMap();   
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.playerEvents.onExperienceGained += PlayerLevelChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.playerEvents.onExperienceGained -= PlayerLevelChange;
    }

    private void Start()
    {
        // broadcast the initial state of all quests on startup
        foreach(Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        // loop through all quests
        foreach(Quest quest in questMap.Values)
        {
            // if meeting the requirements, switch over to CAN_START state
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    private void PlayerLevelChange(int Level) 
    {
        CurrentPlayerLevel = Level;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        if (CurrentPlayerLevel<quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }
        foreach(QuestScriptableObject prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if(GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id)
    {
        Debug.Log("Start Quest: " + id);
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest: " + id);
        Quest quest = GetQuestById(id);

        // move on to the next step
        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(transform);
        }

        // if there are no more steps, it means the quest is ready to be finished
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        Debug.Log("Quest " + quest.info.id + " has been completed.");

        GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.ExperienceReward);
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // load all QuestInfoSOs in path Assets/Resources/Quests
        QuestScriptableObject[] allQuests = Resources.LoadAll<QuestScriptableObject>("Quests");

        // create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach(QuestScriptableObject questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }

            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];

        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }

        return quest;
    }

    public QuestState CheckQuestState(string id)
    {
        Quest quest = GetQuestById(id);

        return quest.state;
    }

    private void OnApplicationQuit()
    {
        foreach(Quest quest in questMap.Values)
        {
            QuestData questData = quest.GetQuestData();
            Debug.Log(quest.info.id);
            Debug.Log("state: " + questData.state);
            Debug.Log("index: " + questData.questStepIndex);
            foreach(QuestStepState stepState in questData.questStepStates)

            {
                Debug.Log("step state: " + stepState.state);
            }
        }
    }
}
