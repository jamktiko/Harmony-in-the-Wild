using System;

public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string id)
    {
        if(onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string> onFinishQuest;

    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

    public event Action<string, int, QuestStepState> onQuestStepStateChange;

    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id, stepIndex, questStepState);
        }
        
    }

    public event Action<string> onAdvanceDungeonQuest;

    public void AdvanceDungeonQuest(string id)
    {
        if (onAdvanceDungeonQuest != null)
        {
            onAdvanceDungeonQuest(id);
        }

    }

    public event Action<int> onShowQuestUI;

    public void ShowQuestUI(int questIndex)
    {
        if (onShowQuestUI != null)
        {
            onShowQuestUI(questIndex);
        }

    }

    public event Action<QuestUIChange, string> onUpdateQuestUI;

    public void UpdateQuestUI(QuestUIChange changeType, string newQuestText)
    {
        if(onUpdateQuestUI != null)
        {
            onUpdateQuestUI(changeType, newQuestText);
        }
    }

    public event Action onHideQuestUI;

    public void HideQuestUI()
    {
        if(onHideQuestUI != null)
        {
            onHideQuestUI();
        }
    }
}
