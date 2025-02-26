using UnityEngine;
using UnityEngine.Serialization;

public class OverrideQuestUI : MonoBehaviour
{
    [FormerlySerializedAs("questName")] public string QuestName;
    [FormerlySerializedAs("description")] public string Description;
    [FormerlySerializedAs("progress")] public string Progress;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(QuestName, Description, Progress);
    }
}
