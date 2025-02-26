using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CollectableQuestStep : QuestStep
{
    [FormerlySerializedAs("itemsCollected")] public int ItemsCollected = 0;
    private int _itemToComplete = 5;
    private List<bool> _itemStates = new List<bool>();
    private List<GameObject> _apples = new List<GameObject>();

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            InitializeQuestUI();
            SetAppleObjects();

            if (_itemStates.Count <= 0)
            {
                InitializeItemStates();
            }
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

            if (_itemStates.Count <= 0)
            {
                InitializeItemStates();
            }
        }
    }

    private void InitializeQuestUI()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress + " " + ItemsCollected + "/" + _itemToComplete);
    }

    public void CollectableProgress(int itemIndex)
    {
        // don't proceed if the index is falsely set or the corresponding item has already been collected
        if (itemIndex < 0 || _itemStates[itemIndex])
        {
            Debug.Log("Invalid item index or corresponding apple has already been collected. Index: " + itemIndex);
            return;
        }

        _itemStates[itemIndex] = true;
        ItemsCollected++;

        UpdateState();
        GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI(Progress + " " + ItemsCollected + "/" + _itemToComplete);

        if (ItemsCollected >= _itemToComplete)
        {
            // hide remaining apples
            //foreach(GameObject apple in apples)
            //{
            //    apple.SetActive(false);
            //}

            FinishQuestStep();
        }
    }

    private void SetAppleObjects()
    {
        _apples = AppleDataHolder.Instance.GetApples();

        if (_apples != null)
        {
            for (int i = 0; i < _apples.Count; i++)
            {
                _apples[i].SetActive(!_itemStates[i]);
            }
        }

        else
        {
            Debug.LogWarning("Was not able to set up apple visibility in Whale Diet, apple list was null.");
        }
    }

    private void InitializeItemStates()
    {
        for (int i = 0; i <= 10; i++)
        {
            _itemStates.Add(false);
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
        string state = ItemsCollected + ";" + string.Join(",", _itemStates);
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        Debug.Log("Setting whale data");
        if (string.IsNullOrEmpty(state))
        {
            Debug.Log("Empty state found");
            InitializeItemStates();
            return;
        }

        string[] stateParts = state.Split(";");

        // if there was previously only one saved value (itemsCollected in older version of the game), initialize the apple values to prevent errors
        if (stateParts.Length == 1)
        {
            ItemsCollected = int.Parse(stateParts[0]);

            if (_itemStates.Count <= 0 || _itemStates == null)
            {
                Debug.Log("Initializing after outdated data format");
                InitializeItemStates();
            }
        }

        else
        {
            Debug.Log("Setting saved values");
            ItemsCollected = ItemsCollected = int.Parse(stateParts[0]);
            _itemStates = ParseItemStates(stateParts[1]);
        }

        UpdateState();
    }
}