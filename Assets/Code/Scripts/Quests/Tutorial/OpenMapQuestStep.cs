using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMapQuestStep : QuestStep
{
    private int mapActionsDone;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
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
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }
    }

    private void Update()
    {
        if (PlayerInputHandler.instance.OpenMapInput.WasPressedThisFrame())
        {
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        mapActionsDone++;

        // if map has been both opened and closed, progress in the quest
        if(mapActionsDone >= 2)
        {
            FinishQuestStep();
        }

        // otherwise just update the quest state
        else
        {
            UpdateState();
        }
    }

    private void UpdateState()
    {
        string state = mapActionsDone.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        mapActionsDone = System.Int32.Parse(state);

        UpdateState();
    }

}
