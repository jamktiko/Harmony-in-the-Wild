using UnityEngine.Serialization;

[System.Serializable]
public class QuestStepState
{
    [FormerlySerializedAs("state")] public string State;

    public QuestStepState(string state)
    {
        this.State = state;
    }

    public QuestStepState()
    {
        State = "";
    }
}
