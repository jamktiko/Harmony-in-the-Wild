using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct QuestImage
{
    public QuestScriptableObject quest;
    public Image displayImage;
}
public class ActiveQuestUI : MonoBehaviour
{

    [SerializeField]List<QuestImage> questImages = new List<QuestImage>();

    [SerializeField]TMP_Text title;
    [SerializeField]TMP_Text description;
    [SerializeField]TMP_Text nextQuestStepDesc;

    public void UpdateQuestMenuUI()
    {
        var activeQuest = QuestMenuManager.trackedQuest;

        title.text = activeQuest.info.displayName.ToUpper();

        description.text = activeQuest.info.description;
        nextQuestStepDesc.text = activeQuest.state == QuestState.IN_PROGRESS? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().objective:"";

        //GameEventsManager.instance.questEvents.ShowQuestUI(activeQuest.info.displayName, activeQuest.state == QuestState.IN_PROGRESS ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().objective : activeQuest.info.description,"");
    }

    public void TrackQuest() 
    {
        var activeQuest = QuestMenuManager.trackedQuest;

        GameEventsManager.instance.questEvents.ShowQuestUI(activeQuest.info.displayName, activeQuest.state == QuestState.IN_PROGRESS ? activeQuest.GetCurrentQuestStepPrefab().GetComponent<QuestStep>().objective : activeQuest.info.description, "");
    }
}
