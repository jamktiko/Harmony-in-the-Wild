using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkers : MonoBehaviour
{
    //list of buttons & quest indicators?
    //index of button matches index of quest
    //when quest finished, disable the corresponding indicator and enable button based on given index

    [SerializeField] private List<GameObject> mapButtons = new List<GameObject>();
    [SerializeField] private List<GameObject> mapIndicators = new List<GameObject>();

    private Dictionary<string, int> idToIndex = new Dictionary<string, int>();
    private Quest quest;
    private int questIndex;

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
        GameEventsManager.instance.questEvents.OnFinishQuest += UnlockMapTeleport;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnFinishQuest -= UnlockMapTeleport;
    }
    void UnlockMapTeleport(string id)
    {
        //Debug.Log("string id is: " + id);

        quest = QuestManager.instance.GetQuestById(id);
        questIndex = GetIndexFromId(quest.info.id);

        //Debug.Log(mapButtons[questIndex]);
        //Debug.Log(mapIndicators[questIndex]);
        mapButtons[questIndex].SetActive(true);
        mapIndicators[questIndex].SetActive(false);

        //Debug.Log("QuestMarker says: questIndex - " + questIndex);

        foreach (var pair in idToIndex)
        {
            //Debug.Log("Key: " + pair.Key + ", Value: " + pair.Value);
        }

    }
    private void Update()
    {
        int questIndex = 0;

        foreach (Quest quest in QuestManager.instance.questMap.Values)
        {
            if (quest.state == QuestState.FINISHED)
            {
                questIndex = GetIndexFromId(quest.info.id);
                //Debug.Log($"QuestMarker says: quest.info.id is - {quest.info.id}. questIndex is - {questIndex}.");
                mapButtons[questIndex].SetActive(true);
                mapIndicators[questIndex].SetActive(false);
            }
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
