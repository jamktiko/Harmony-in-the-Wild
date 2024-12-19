using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class CollectableQuestStep : QuestStep
{
    public int itemsCollected = 0;
    private int itemToComplete = 5;
    private List<bool> itemStates = new List<bool>();
    private List<GameObject> apples = new List<GameObject>();

    private void Start()
    {
        InitializeQuestUI();

        if(SceneManager.GetActiveScene().name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            SetAppleObjects();
            InitializeItemStates();
        }
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
            InitializeQuestUI();
            SetAppleObjects();
            InitializeItemStates();
        }
    }

    private void InitializeQuestUI()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress + " " + itemsCollected + "/" + itemToComplete);
    }

    public void CollectableProgress(int itemIndex)
    {
        // don't proceed if the index is falsely set or the corresponding item has already been collected
        if(itemIndex < 0 || itemIndex >= itemToComplete || itemStates[itemIndex])
        {
            Debug.Log("Invalid item index or corresponding apple has already been collected.");
            return;
        }

        itemStates[itemIndex] = true;
        itemsCollected++;

        UpdateState();
        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI(progress + " " + itemsCollected + "/" + itemToComplete);

        if (itemsCollected >= itemToComplete)
        {
            // hide remaining apples
            foreach(GameObject apple in apples)
            {
                apple.SetActive(false);
            }

            FinishQuestStep();
        }
    }

    private void SetAppleObjects()
    {
        apples = AppleDataHolder.instance.GetApples();

        if(apples != null)
        {
            for(int i = 0; i < apples.Count; i++)
            {
                apples[i].SetActive(!itemStates[i]);
            }
        }

        else
        {
            Debug.LogWarning("Was not able to set up apple visibility in Whale Diet, apple list was null.");
        }
    }

    private void InitializeItemStates()
    {
        foreach(GameObject apple in apples)
        {
            itemStates.Add(false);
        }
    }

    private List<bool> ParseItemStates(string stateString)
    {
        List<bool> states = new List<bool>();
        string[] stateValues = stateString.Split(',');

        foreach (var value in stateValues)
        {
            states.Add(bool.Parse(value));
        }

        return states;
    }

    private void UpdateState()
    {
        string state = itemsCollected + ";" + string.Join(",", itemStates);
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        if (string.IsNullOrEmpty(state))
        {
            InitializeItemStates();
            return;
        }

        string[] stateParts = state.Split(";");

        // if there was previously only one saved value (itemsCollected in older version of the game), initialize the apple values to prevent errors
        if(stateParts.Length == 1)
        {
            itemsCollected = int.Parse(stateParts[0]);
            InitializeItemStates();
        }

        else
        {
            itemsCollected = itemsCollected = int.Parse(stateParts[0]);
            itemStates = ParseItemStates(stateParts[1]);
        }

        UpdateState();
    }
}