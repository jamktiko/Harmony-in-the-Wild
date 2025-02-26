using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public struct QuestImage
{
    [FormerlySerializedAs("quest")] public QuestScriptableObject Quest;
    [FormerlySerializedAs("displayImage")] public Image DisplayImage;
}
public class ActiveQuestUI : MonoBehaviour
{

    [FormerlySerializedAs("questImages")] [SerializeField] List<QuestImage> _questImages = new List<QuestImage>();

    [FormerlySerializedAs("questWaypoint")] [SerializeField] QuestWaypoint _questWaypoint;

    [FormerlySerializedAs("title")] [SerializeField] TMP_Text _title;
    [FormerlySerializedAs("description")] [SerializeField] TMP_Text _description;
    [SerializeField] TMP_Text _nextQuestStepDesc;

    private void Awake()
    {
        _questWaypoint = FindObjectOfType<QuestWaypoint>();
    }
    public void UpdateQuestMenuUI()
    {
        var activeQuest = QuestMenuManager.TrackedQuest;

        _title.text = activeQuest.Info.DisplayName.ToUpper();

        _description.text = activeQuest.Info.Description;
        _nextQuestStepDesc.text = activeQuest.State == QuestState.InProgress ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().Objective : "";

        //GameEventsManager.instance.questEvents.ShowQuestUI(activeQuest.info.displayName, activeQuest.state == QuestState.IN_PROGRESS ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().objective : activeQuest.info.description,"");
    }

    public void TrackQuest()
    {
        var activeQuest = QuestMenuManager.TrackedQuest;

        _questWaypoint.GetNewQuestWaypointPosition();

        GameEventsManager.instance.QuestEvents.ShowQuestUI(activeQuest.Info.DisplayName, activeQuest.State == QuestState.InProgress ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().Objective : activeQuest.Info.Description, activeQuest.Info.DisplayName);
    }
}
