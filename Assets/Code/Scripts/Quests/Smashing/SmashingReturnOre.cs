using UnityEngine;
using UnityEngine.Serialization;

public class SmashingReturnOre : QuestStep
{
    [FormerlySerializedAs("hasOre")] [SerializeField] private bool _hasOre;

    public static SmashingReturnOre Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Return Ore.");
        }

        Instance = this;
    }

    public void PickUpOre()
    {
        _hasOre = true;
        FinishQuestStep();
    }

    private void UpdateState()
    {
        string state = _hasOre.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _hasOre = System.Convert.ToBoolean(state);

        UpdateState();
    }
}
