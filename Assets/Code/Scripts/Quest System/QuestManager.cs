using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Dictionary<string, Quest> questMap;
    private int curID = 0;

    public static QuestManager instance;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AbilityCycle AbilityCycle;
    [SerializeField] private Sprite[] mapQuestMarkersBW;
    [SerializeField] private Sprite[] mapQuestMarkersColour;

    private int currentPlayerLevel;
    private string currentActiveQuest;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }

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
        CheckAllRequirements();

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

            Debug.Log(quest.info.id + ": state is set to " + quest.state);
        }

        if (SceneManager.GetActiveScene().name == "Overworld" && AbilityCycle == null)
        {
            AbilityCycle = FindObjectOfType<AbilityCycle>(); // In case Overworld is loaded in editor, find AbilityCycle
        }
    }

    public void SetQuestMarkers(Image[] mapQuestMarkers)
    {
        if (questMap != null)
        {
            foreach (KeyValuePair<string, Quest> quest in questMap)
            {
                if (quest.Value.info.numericID < mapQuestMarkers.Length)
                {
                    if (quest.Value.state == QuestState.REQUIREMENTS_NOT_MET)
                        mapQuestMarkers[quest.Value.info.numericID].sprite = mapQuestMarkersBW[quest.Value.info.numericID];
                    else
                        mapQuestMarkers[quest.Value.info.numericID].sprite = mapQuestMarkersColour[quest.Value.info.numericID];
                }
            }
        }
    }

    private void PlayerLevelChange(int Level) 
    {
        currentPlayerLevel = Level;
    }

    public void CheckAllRequirements()
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

        /*if (currentPlayerLevel<quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }*/
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

            if(id != "Tutorial")
            {
                AbilityAcquired(quest.info.abilityReward);
                //Debug.Log("Ability unlocked: " + quest.info.abilityReward);
            }
        }
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
            //Debug.Log("Quest " + id + " state advanced.");
            quest.InstantiateCurrentQuestStep(transform);
        }

        // if there are no more steps, it means the quest is ready to  be finished
        else
        {
            // if you are finishing a side quest, call the event that will enable showing the final quest UI for that side quest
            if (!quest.info.mainQuest)
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
                GameEventsManager.instance.questEvents.ReturnToSideQuestPoint(id);
            }

            // if you are finishing a main quest, instantly finish it since you won't need to return to any quest point for the actual finish
            else
            {
                Debug.Log("About to finish dungeon quest: " + id);
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
                //GameEventsManager.instance.questEvents.FinishQuest(id);
            }
        }
    }

    private void FinishQuest(string id)
    {
        //Debug.Log("Finished quest with ID: "+ id);
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        AudioManager.instance.PlaySound(AudioName.Action_QuestCompleted, transform);
        GameEventsManager.instance.questEvents.HideQuestUI();
        QuestCompletedUI.instance.ShowUI(id);
        ResetActiveQuest();
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

    private void SetActiveQuest(string id)
    {
        currentActiveQuest = id;
    }

    private void ResetActiveQuest()
    {
        currentActiveQuest = "";
    }

    public string GetActiveQuest()
    {
        return currentActiveQuest;
    }

    public Dictionary<string, Quest> CreateQuestMap()
    {
        if(questMap != null)
        {
            Debug.Log("Reset quest map");
            questMap = null;

            foreach(Transform questStep in transform)
            {
                Destroy(questStep.gameObject);
            }
        }

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

    private void SubscribeToEvents()
    {
        GameEventsManager.instance.questEvents.OnStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.OnAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest += FinishQuest;

        GameEventsManager.instance.questEvents.OnQuestStepStateChange += QuestStepStateChange;

        GameEventsManager.instance.playerEvents.OnExperienceGained += PlayerLevelChange;
        GameEventsManager.instance.playerEvents.OnAbilityAcquired += AbilityAcquired;

        GameEventsManager.instance.questEvents.OnChangeActiveQuest += SetActiveQuest;
    }

    private void UnsubscribeFromEvents()
    {
        GameEventsManager.instance.questEvents.OnStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.OnFinishQuest -= FinishQuest;

        GameEventsManager.instance.questEvents.OnQuestStepStateChange -= QuestStepStateChange;

        GameEventsManager.instance.playerEvents.OnExperienceGained -= PlayerLevelChange;
        GameEventsManager.instance.playerEvents.OnAbilityAcquired -= AbilityAcquired;

        GameEventsManager.instance.questEvents.OnChangeActiveQuest -= SetActiveQuest;
    }
    public void RequestFinishQuest(string id) // For some reason the event doesn't trigger reliably so as a workaround to ensure dungeons finish, this is called.
    {
        FinishQuest(id);
    }
}
