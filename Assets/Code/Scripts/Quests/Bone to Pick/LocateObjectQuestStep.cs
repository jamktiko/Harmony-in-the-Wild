using UnityEngine;

public class LocateObjectQuestStep : QuestStep
{
    [Header("Object to Find")]
    [SerializeField] private QuestItem itemToFind;
    [SerializeField] private int amountOfItemsToFind = 1;

    private int currentAmountOfFoundItems = 0;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnCollectQuestItem += CheckItemMatch;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnCollectQuestItem -= CheckItemMatch;
    }

    private void CheckItemMatch(QuestItem foundItem)
    {
        if (foundItem == itemToFind)
        {
            currentAmountOfFoundItems++;

            if (currentAmountOfFoundItems >= amountOfItemsToFind)
            {
                FinishQuestStep();
            }

            else
            {
                UpdateState();
            }
        }
    }

    private void UpdateState()
    {
        string state = currentAmountOfFoundItems.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        currentAmountOfFoundItems = int.Parse(state);

        UpdateState();
    }
}