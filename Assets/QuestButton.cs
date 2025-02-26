using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    [SerializeField]ActiveQuestUI activeQuestUI;
    private void Awake()
    {
        //In case there are several activeQuestUI objects in the scene, we need to find the right one
        var activeQuestUIList = FindObjectsOfType<ActiveQuestUI>(true).ToList();
        activeQuestUI = activeQuestUIList.Count > 1 ? activeQuestUIList[1] : activeQuestUIList[0];

        if (QuestMenuManager.trackedQuest != null && !activeQuestUI.gameObject.activeSelf)
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
    }
}
