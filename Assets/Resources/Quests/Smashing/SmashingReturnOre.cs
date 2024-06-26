using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingReturnOre : QuestStep
{
    [SerializeField] private bool hasOre;

    public static SmashingReturnOre instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Return Ore.");
        }

        instance = this;
    }

    public void PickUpOre()
    {
        hasOre = true;
        FinishQuestStep();
        //GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
    }

    private void UpdateState()
    {
        string state = hasOre.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        hasOre = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
