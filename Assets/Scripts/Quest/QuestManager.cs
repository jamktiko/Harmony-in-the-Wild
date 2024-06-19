using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public Dictionary<string, Quest> questMap;

    public static QuestManager instance;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AbilityCycle AbilityCycle;

    private int currentPlayerLevel;

    ShowQuestUI questUI;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Quest Manager.");
            Destroy(gameObject);
        }
        else
        instance = this;

        // initialize quest map
        //questMap = CreateQuestMap();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
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

        if(SceneManager.GetActiveScene().name == "Overworld" && AbilityCycle == null)
        {
            AbilityCycle = FindObjectOfType<AbilityCycle>(); // In case Overworld is loaded in editor, find AbilityCycle
        }
    }

    private void Update()
    {
        //// loop through all quests
        //foreach(Quest quest in questMap.Values)
        //{
        //    // if meeting the requirements, switch over to CAN_START state
        //    if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
        //    {
        //        ChangeQuestState(quest.info.id, QuestState.CAN_START);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (var quest in questMap.Values)
            {
                Debug.Log(quest.info.name+" "+quest.state);
            }
        }
    }

    private void PlayerLevelChange(int Level) 
    {
        currentPlayerLevel = Level;
    }

    private void CheckAllRequirements()
    {
        // loop through all quests
        foreach (Quest quest in questMap.Values)
        {
            // if meeting the requirements, switch over to CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    private bool CheckRequirementsMet(Quest quest)
    {
        // start true and prove to be false
        bool meetsRequirements = true;

        if (currentPlayerLevel<quest.info.levelRequirement)
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
        Debug.Log("Changed quest state with ID: " + id + " to: " + state.ToString());
        Quest quest = GetQuestById(id);
        quest.state = state;
        //Debug.Log(quest.state.ToString());
        SaveManager.instance.SaveGame(); // Force save game when quest state changes
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest.state == QuestState.CAN_START)
        {
            //Debug.Log("Start Quest: " + id);
            quest.InstantiateCurrentQuestStep(transform);
            ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
            AbilityAcquired(quest.info.abilityReward);
            //Debug.Log("Ability unlocked: " + quest.info.abilityReward);
        }
    }

    private void AdvanceQuest(string id)
    {
        //Debug.Log("Advance Quest: " + id);
        Quest quest = GetQuestById(id);

        // move on to the next step
        quest.MoveToNextStep();

        // if there are more steps, instantiate the next one
        if (quest.CurrentStepExists())
        {   
            //Debug.Log("Quest " + id + " state advanced.");
            quest.InstantiateCurrentQuestStep(transform);
        }

        // if there are no more steps, it means the quest is ready to  be finished
        else
        {
            //Debug.Log("Quest " + id + " state requested to can finish.");
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        //Debug.Log("Finished quest with ID: "+ id);
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        QuestCompletedUI.instance.ShowUI(id);
        CheckAllRequirements();
    }

    private void ClaimRewards(Quest quest)
    {
        //Debug.Log("Quest " + quest.info.id + " has been completed.");

        GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);

        if (quest.info.abilityReward != 0)
        {
            AbilityAcquired(quest.info.abilityReward);
            //StartCoroutine(AbilityCycle.MakeList());
        }
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    public Dictionary<string, Quest> CreateQuestMap()
    {
        // load all QuestInfoSOs in path Assets/Resources/Quests
        QuestScriptableObject[] allQuests = Resources.LoadAll<QuestScriptableObject>("Quests");

        // create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        // load loaded data from Save Manager
        List<string> loadedQuestData = SaveManager.instance.GetLoadedQuestData("quest");

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

    private void AbilityAcquired(Abilities ability) 
    {
        AbilityManager.instance.UnlockAbility(ability);

        if (ability == Abilities.GhostSpeaking)
        {
            ActivateGhostSpeak();
        }
    }
    public void ActivateGhostSpeak()
    {
        GameEventsManager.instance.playerEvents.GhostSpeakActivated();
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
        //UnsubscribeFromEvents();
        //SubscribeToEvents();

        //Debug.Log("Currently loaded level: "+ level);
        ////questMap = CreateQuestMap();
        //playerManager = FindObjectOfType<PlayerManager>();
        
        //// broadcast the initial state of all quests on startup
        //foreach (Quest quest in questMap.Values)
        //{

        //    // broadcast the initial state of all quests
        //    GameEventsManager.instance.questEvents.QuestStateChange(quest);
        //}
        if (level != 0 || level != 1 || level != 9 || level != 2 || level != 11)
        {
            AbilityCycle = FindObjectOfType<AbilityCycle>();
        }

        CheckAllRequirements();

        questUI = FindObjectOfType<ShowQuestUI>();
    }

    private void SubscribeToEvents()
    {
        //Debug.Log("Subscribing to quest events...");
        GameEventsManager.instance.questEvents.OnStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.OnAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.OnQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.playerEvents.OnExperienceGained += PlayerLevelChange;
        GameEventsManager.instance.playerEvents.OnAbilityAcquired += AbilityAcquired;
    }

    private void UnsubscribeFromEvents()
    {
        Debug.Log("Un-subscribing from quest events...");
        GameEventsManager.instance.questEvents.OnStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.OnQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.playerEvents.OnExperienceGained -= PlayerLevelChange;
        GameEventsManager.instance.playerEvents.OnAbilityAcquired -= AbilityAcquired;
    }
    public void RequestFinishQuest(string id) // For some reason the event doesn't trigger reliably so as a workaround to ensure dungeons finish, this is called.
    {
        FinishQuest(id);
    }
}
