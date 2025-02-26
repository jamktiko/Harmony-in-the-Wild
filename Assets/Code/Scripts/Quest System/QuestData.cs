using UnityEngine.Serialization;

[System.Serializable]
public class QuestData
{
    [FormerlySerializedAs("state")] public QuestState State;
    [FormerlySerializedAs("questStepIndex")] public int QuestStepIndex;
    [FormerlySerializedAs("questStepStates")] public QuestStepState[] QuestStepStates;

    public QuestData(QuestState state, int questStepIndex, QuestStepState[] questStepStates)
    {
        this.State = state;
        this.QuestStepIndex = questStepIndex;
        this.QuestStepStates = questStepStates;
    }
}
