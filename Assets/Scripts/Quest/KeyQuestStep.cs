using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyQuestStep : QuestStep
{
    [SerializeField] int number;
    public int itemsCollected = 0;
    private int itemToComplete = 1;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Key" + number).GetComponent<InteractableKey1>().used) 
        {
            CollectableProgress();

        }
    }

    // Update is called once per frame
    public void CollectableProgress()
    {
        itemsCollected++;
        UpdateState();

        if (itemsCollected >= itemToComplete)
        {
            FinishQuestStep();
        }
    }
    private void UpdateState()
    {
        string state = itemsCollected.ToString();
        ChangeState(state);
    }
    protected override void SetQuestStepState(string state)
    {
        itemsCollected = System.Int32.Parse(state);
        UpdateState();
    }
}
