using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public struct QuestImage
{
    public QuestScriptableObject Quest;
    public Image DisplayImage;
}
public class ActiveQuestUI : MonoBehaviour
{

    [SerializeField] private List<QuestImage> _questImages = new List<QuestImage>();

    [SerializeField] private QuestWaypoint _questWaypoint;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _nextQuestStepDesc;
    
    [SerializeField] private Button _trackQuestButton;
    [SerializeField] private Button _untrackQuestButton;
    
    private Quest _trackedQuest;

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
        
        if (_trackedQuest!=null && _title.text == _trackedQuest.Info.DisplayName.ToUpperInvariant())
        {
            _trackQuestButton.gameObject.SetActive(false);
            _untrackQuestButton.gameObject.SetActive(true);
        }
        else
        {
            _trackQuestButton.gameObject.SetActive(true);
            _untrackQuestButton.gameObject.SetActive(false);
        }

        //GameEventsManager.instance.questEvents.ShowQuestUI(activeQuest.info.displayName, activeQuest.state == QuestState.IN_PROGRESS ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().objective : activeQuest.info.description,"");
    }

    public void TrackQuest()
    {
        var activeQuest = QuestMenuManager.TrackedQuest;

        _questWaypoint.GetNewQuestWaypointPosition();

        GameEventsManager.instance.QuestEvents.ShowQuestUI(activeQuest.Info.DisplayName, activeQuest.State == QuestState.InProgress ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().Objective : activeQuest.Info.Description, activeQuest.Info.DisplayName);
        
        _trackedQuest = activeQuest;
    }

    public void UntrackQuest()
    {
        _questWaypoint.GetNewQuestWaypointPosition(false);
        
        _trackedQuest = null;
        
        GameEventsManager.instance.QuestEvents.HideQuestUI();
    }
}
