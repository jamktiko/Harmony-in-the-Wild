using UnityEngine;

public class Quest
{
    public QuestScriptableObject info;

    public QuestState state;

    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public Quest(QuestScriptableObject questInfo)
    {
        info = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
        questStepStates = new QuestStepState[info.questStepPrefabs.Length];

        // initialize all the quest step states so they won't be null in the beginning
        for(int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestScriptableObject questInfo, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        info = questInfo;
        state = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        // if the quest step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync
        if(this.questStepStates.Length != this.info.questStepPrefabs.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are " +
                "of different lengths. This indicates something changed with the " +
                "QuestInfo and the saved data is now out of sync. Reset your data," +
                "as this might cause issues. Quest Id: " + this.info.id);
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < info.questStepPrefabs.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if(questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }

        else
        {
            Debug.LogWarning("Quest step index out of range.");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if(stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
        }

        else
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: Quest Id = " + info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }

    public int GetCurrentQuestStepIndex()
    {
        return currentQuestStepIndex;
    }
}
