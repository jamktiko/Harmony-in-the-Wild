using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    [SerializeField]ActiveQuestUI activeQuestUI;
    private void Awake()
    {
        var activeQuestUIList = FindObjectsOfType<ActiveQuestUI>(true).ToList();

        //Determine whether we have 2 managers in the scene for testing purposes 
        if (activeQuestUIList.Count>1)
        {
            activeQuestUI = activeQuestUIList[1];
        }
        else
        {
            activeQuestUI = activeQuestUIList[0];
        }

        if (QuestMenuManager.trackedQuest!=null && !activeQuestUI.gameObject.activeSelf)
        {
            activeQuestUI.gameObject.SetActive(true);
            activeQuestUI.UpdateQuestMenuUI();
        }
    }
    public void ChooseQuest()
    {
        string name = gameObject.name;
        Quest thisQuest;
        var questList  = QuestManager.instance.questMap.Where(x => name.Contains(x.Value.info.displayName)).Select(x => x.Value).ToList();
        thisQuest= questList.FirstOrDefault();

        QuestMenuManager.trackedQuest = thisQuest;

        activeQuestUI.gameObject.SetActive(true);
        activeQuestUI.UpdateQuestMenuUI();

        Debug.Log(QuestMenuManager.trackedQuest.info.displayName);
    }
}
