using System.Collections.Generic;
using UnityEngine;

public class QuestSearchArea : MonoBehaviour
{
    [SerializeField] private GameObject whaleSearchArea;
    [SerializeField] private GameObject ghostSearchArea;
    [SerializeField] private GameObject boneSearchArea;

    private Dictionary<string, int> idToIndex = new Dictionary<string, int>();
    private Quest quest;

    private void Start()
    {
        Invoke("GrabQuestIds", 1f);
    }

    void GrabQuestIds()
    {
        int index = 0;
        foreach (string questId in QuestManager.instance.questMap.Keys)
        {
            idToIndex.Add(questId, index);

            index++;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest += ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.questEvents.OnFinishQuest += ToggleSearchAreaOnMinimap;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnStartQuest -= ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.questEvents.OnFinishQuest -= ToggleSearchAreaOnMinimap;
    }

    public void ToggleSearchAreaOnMinimap(string id)
    {
        quest = QuestManager.instance.GetQuestById(id);
        int questIndex = GetIndexFromId(quest.info.id);

        ToggleSearchArea(questIndex);
    }

    private void ToggleSearchArea(int questIndex)
    {
        //Whale == 16
        //Ghost == 4
        //Bone == 1

        if (questIndex == 16)
        {
            whaleSearchArea.SetActive(!whaleSearchArea.activeInHierarchy);
        }
        else if (questIndex == 4)
        {
            ghostSearchArea.SetActive(!ghostSearchArea.activeInHierarchy);
        }
        else if (questIndex == 1)
        {
            boneSearchArea.SetActive(!boneSearchArea.activeInHierarchy);
        }
    }

    int GetIndexFromId(string id)
    {
        if (idToIndex.ContainsKey(id))
        {
            return idToIndex[id];
        }
        else
        {
            throw new KeyNotFoundException("ID not found: " + id);
        }
    }
}
