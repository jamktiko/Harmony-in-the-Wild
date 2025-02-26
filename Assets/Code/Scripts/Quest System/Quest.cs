using UnityEngine;

public class Quest
{
    public QuestScriptableObject Info;

    public QuestState State;

    private int _currentQuestStepIndex;
    private QuestStepState[] _questStepStates;

    public Vector3 DefaultPosition;
    public Quest(QuestScriptableObject questInfo)
    {
        Info = questInfo;
        State = QuestState.RequirementsNotMet;
        DefaultPosition = Info.DefaultPos;
        _currentQuestStepIndex = 0;
        _questStepStates = new QuestStepState[Info.QuestStepPrefabs.Length];

        // initialize all the quest step states so they won't be null in the beginning
        for (int i = 0; i < _questStepStates.Length; i++)
        {
            _questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestScriptableObject questInfo, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        Info = questInfo;
        State = questState;
        this._currentQuestStepIndex = currentQuestStepIndex;
        this._questStepStates = questStepStates;
        this.DefaultPosition = Info.DefaultPos;
        // if the quest step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync
        if (this._questStepStates.Length != this.Info.QuestStepPrefabs.Length)
        {
            Debug.LogWarning("Quest Step Prefabs and Quest Step States are " +
                "of different lengths. This indicates something changed with the " +
                "QuestInfo and the saved data is now out of sync. Reset your data," +
                "as this might cause issues. Quest Id: " + this.Info.id);
        }
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return _currentQuestStepIndex < Info.QuestStepPrefabs.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(Info.id, _currentQuestStepIndex, _questStepStates[_currentQuestStepIndex].State);
        }
    }

    public GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = Info.QuestStepPrefabs[_currentQuestStepIndex];
        }

        else
        {
            Debug.LogWarning("Quest step index out of range.");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < _questStepStates.Length)
        {
            _questStepStates[stepIndex].State = questStepState.State;
        }

        else
        {
            Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: Quest Id = " + Info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(State, _currentQuestStepIndex, _questStepStates);
    }

    public int GetCurrentQuestStepIndex()
    {
        return _currentQuestStepIndex;
    }
}
