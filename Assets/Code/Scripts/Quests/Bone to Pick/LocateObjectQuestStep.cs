using UnityEngine;
using UnityEngine.Serialization;

public class LocateObjectQuestStep : QuestStep
{
    [FormerlySerializedAs("itemToFind")]
    [Header("Object to Find")]
    [SerializeField] private QuestItem _itemToFind;
    [FormerlySerializedAs("amountOfItemsToFind")] [SerializeField] private int _amountOfItemsToFind = 1;

    private int _currentAmountOfFoundItems = 0;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnCollectQuestItem += CheckItemMatch;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnCollectQuestItem -= CheckItemMatch;
    }

    private void CheckItemMatch(QuestItem foundItem)
    {
        if (foundItem == _itemToFind)
        {
            _currentAmountOfFoundItems++;

            if (_currentAmountOfFoundItems >= _amountOfItemsToFind)
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
        string state = _currentAmountOfFoundItems.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _currentAmountOfFoundItems = int.Parse(state);

        UpdateState();
    }
}