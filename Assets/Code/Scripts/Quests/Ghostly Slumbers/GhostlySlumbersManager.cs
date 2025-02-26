using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostlySlumbersManager : QuestStep
{
    [FormerlySerializedAs("relativeInteractionStatus")]
    [Header("Debug")]
    [SerializeField] private List<bool> _relativeInteractionStatus;

    public static GhostlySlumbersManager Instance;

    private int _relativesSpokenTo = 0;

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Ghostly Slumbers Manager.");
        }

        Instance = this;

        List<GameObject> relatives = GhostRelatives.Instance.GetGhostRelatives();

        foreach (GameObject relative in relatives)
        {
            relative.SetActive(true);
        }

        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress + " " + _relativesSpokenTo + "/" + _relativeInteractionStatus.Count);

    }

    public void TalkedToRelative(int relativeIndex)
    {
        _relativeInteractionStatus[relativeIndex] = true;
        _relativesSpokenTo++;

        GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI(Progress + " " + _relativesSpokenTo + "/" + _relativeInteractionStatus.Count);

        if (_relativesSpokenTo < _relativeInteractionStatus.Count)
        {
            UpdateState();
        }

        else if (_relativesSpokenTo >= _relativeInteractionStatus.Count)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = _relativeInteractionStatus.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        string[] splitState = state.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < splitState.Length; i++)
        {
            _relativeInteractionStatus[i] = System.Convert.ToBoolean(splitState[i]);
        }

        UpdateState();
    }
}
