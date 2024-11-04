using System;
using System.Diagnostics;

public class QuestEvents
{
    public event Action<string> OnStartQuest;

    public void StartQuest(string id)
    {
        OnStartQuest?.Invoke(id);
    }

    public event Action<string> OnAdvanceQuest;

    public void AdvanceQuest(string id)
    {
        OnAdvanceQuest?.Invoke(id);
        UnityEngine.Debug.Log("Invoked advance quest");

    }

    public event Action<string> OnFinishQuest;

    public void FinishQuest(string id)
    {
        OnFinishQuest?.Invoke(id);
        UnityEngine.Debug.Log("Invoked finish quest");

    }

    public event Action<Quest> OnQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        OnQuestStateChange?.Invoke(quest);
    }

    public event Action<string, int, QuestStepState> OnQuestStepStateChange;

    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        OnQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
    }

    public event Action<string> OnAdvanceDungeonQuest;

    public void AdvanceDungeonQuest(string id)
    {
        OnAdvanceDungeonQuest?.Invoke(id);
    }

    public event Action<string, string, string> OnShowQuestUI;

    public void ShowQuestUI(string questName, string description, string progress)
    {
        OnShowQuestUI?.Invoke(questName, description, progress);
    }

    public event Action<string> OnUpdateQuestProgressInUI;

    public void UpdateQuestProgressInUI(string progress)
    {
        OnUpdateQuestProgressInUI?.Invoke(progress);
    }

    public event Action OnHideQuestUI;

    public void HideQuestUI()
    {
        OnHideQuestUI?.Invoke();
    }

    public event Action<string> OnReturnToSideQuestPoint;

    public void ReturnToSideQuestPoint(string id)
    {
        OnReturnToSideQuestPoint?.Invoke(id);
    }

    public event Action<string> OnChangeActiveQuest;

    public void ChangeActiveQuest(string id)
    {
        OnChangeActiveQuest?.Invoke(id);
    }

    public event Action OnStartMovingWhale;

    public void StartMovingWhale()
    {
        OnStartMovingWhale?.Invoke();
    }

    public event Action OnReachWhaleDestination;

    public void ReachWhaleDestination()
    {
        OnReachWhaleDestination?.Invoke();
    }
}
