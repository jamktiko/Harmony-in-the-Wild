using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    public static QuestManager instance;

    private int CurrentPlayerLevel;

    [SerializeField]private PlayerManager playerManager;

    [SerializeField] private AbilityCycle AbilityCycle;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Quest Manager.");
            Destroy(gameObject);
        }

        instance = this;

        // initialize quest map
        //questMap = CreateQuestMap();
        //playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.playerEvents.onExperienceGained += PlayerLevelChange;
        GameEventsManager.instance.playerEvents.onAbilityGet += AbilityGet;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.playerEvents.onExperienceGained -= PlayerLevelChange;
        GameEventsManager.instance.playerEvents.onAbilityGet -= AbilityGet;
    }
    private void Start()
    {
        questMap = CreateQuestMap();
        playerManager = FindObjectOfType<PlayerManager>();
        // broadcast the initial state of all quests on startup
        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if(quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(transform);
            }

            // broadcast the initial state of all quests
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
        AbilityGet(quest.info.AbilityReward);
        StartCoroutine(AbilityCycle.MakeList());
        Debug.Log("Ability unlocked: " + quest.info.AbilityReward);
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
        QuestCompletedUI.instance.ShowUI(id);
    }

    private void ClaimRewards(Quest quest)
    {
        Debug.Log("Quest " + quest.info.id + " has been completed.");

        GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.ExperienceReward);
        AbilityGet(quest.info.AbilityReward);
        StartCoroutine(AbilityCycle.MakeList());
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
        Debug.Log(allQuests.Length);

        // create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        // load loaded data from Save Manager
        List<string> loadedQuestData = SaveManager.instance.FetchLoadedData("quest");

        int currentQuestSOIndex = 0;

        foreach (QuestScriptableObject questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }

            if(loadedQuestData.Count > 0)
            {
                idToQuestMap.Add(questInfo.id, LoadQuest(questInfo, loadedQuestData[currentQuestSOIndex]));
            }

            else
            {
                idToQuestMap.Add(questInfo.id, LoadQuest(questInfo, null));
            }

            currentQuestSOIndex++;
        }

        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
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

    private void AbilityGet(int index) 
    {
        playerManager.abilityValues[index] = true;
        SaveManager.instance.SaveGame();
    }

    public List<string> CollectQuestDataForSaving()
    {
        List<string> allQuestData = new List<string>();
        int i = 0;

        foreach(Quest quest in questMap.Values)
        {
            string savedQuestData = GetSerializedQuestData(quest);
            allQuestData.Add(savedQuestData);

            i++;
        }

        return allQuestData;
    }

    private string GetSerializedQuestData(Quest quest)
    {
        string serializedData = "";

        try
        {
            QuestData questData = quest.GetQuestData();
            serializedData = JsonUtility.ToJson(questData);
        }

        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
        }

        return serializedData;
    }

    private Quest LoadQuest(QuestScriptableObject questInfo, string serializedData)
    {
        Quest quest = null;

        try
        {
            QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
            quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
        }

        catch
        {
            quest = new Quest(questInfo);
        }

        return quest;
    }
    private void OnLevelWasLoaded(int level)
    {
        questMap = CreateQuestMap();
        playerManager = FindObjectOfType<PlayerManager>();
        
        // broadcast the initial state of all quests on startup
        foreach (Quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(transform);
            }

            // broadcast the initial state of all quests
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
        if (level != 0 || level != 1 || level != 9 || level != 2 || level != 11)
        {
            AbilityCycle = FindObjectOfType<AbilityCycle>();
        }
    }
}
