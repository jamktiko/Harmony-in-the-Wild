using System.Collections.Generic;
using UnityEngine;

public class GhostlySlumbersManager : QuestStep
{
    [Header("Debug")]
    [SerializeField] private List<bool> relativeInteractionStatus;

    public static GhostlySlumbersManager instance;

    private int relativesSpokenTo = 0;

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Ghostly Slumbers Manager.");
        }

        instance = this;

        List<GameObject> relatives = GhostRelatives.instance.GetGhostRelatives();

        foreach (GameObject relative in relatives)
        {
            relative.SetActive(true);
        }

        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress + " " + relativesSpokenTo + "/" + relativeInteractionStatus.Count);

    }

    public void TalkedToRelative(int relativeIndex)
    {
        relativeInteractionStatus[relativeIndex] = true;
        relativesSpokenTo++;

        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI(progress + " " + relativesSpokenTo + "/" + relativeInteractionStatus.Count);

        if (relativesSpokenTo < relativeInteractionStatus.Count)
        {
            UpdateState();
        }

        else if (relativesSpokenTo >= relativeInteractionStatus.Count)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = relativeInteractionStatus.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        string[] splitState = state.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < splitState.Length; i++)
        {
            relativeInteractionStatus[i] = System.Convert.ToBoolean(splitState[i]);
        }

        UpdateState();
    }
}
