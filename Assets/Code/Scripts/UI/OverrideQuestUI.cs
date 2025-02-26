using UnityEngine;

public class OverrideQuestUI : MonoBehaviour
{
    public string questName;
    public string description;
    public string progress;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(questName, description, progress);
    }
}
