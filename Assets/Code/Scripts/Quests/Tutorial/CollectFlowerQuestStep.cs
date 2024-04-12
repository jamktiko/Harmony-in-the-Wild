using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFlowerQuestStep : QuestStep
{
    public static CollectFlowerQuestStep instance;

    private bool collectedFlower;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one Collect Flower Quest Steps in the scene!");
        }

        else
        {
            instance = this;
        }
    }

    public void CollectFlower()
    {
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = collectedFlower.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        collectedFlower = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
