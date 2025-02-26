using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestSearchArea : MonoBehaviour
{
    [FormerlySerializedAs("whaleSearchArea")] [SerializeField] private GameObject _whaleSearchArea;
    [FormerlySerializedAs("ghostSearchArea")] [SerializeField] private GameObject _ghostSearchArea;
    [FormerlySerializedAs("boneSearchArea")] [SerializeField] private GameObject _boneSearchArea;

    private Dictionary<string, int> _idToIndex = new Dictionary<string, int>();
    private Quest _quest;

    private void Start()
    {
        Invoke("GrabQuestIds", 1f);
    }

    void GrabQuestIds()
    {
        int index = 0;
        foreach (string questId in QuestManager.Instance.QuestMap.Keys)
        {
            _idToIndex.Add(questId, index);

            index++;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest += ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.QuestEvents.OnFinishQuest += ToggleSearchAreaOnMinimap;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnStartQuest -= ToggleSearchAreaOnMinimap;
        GameEventsManager.instance.QuestEvents.OnFinishQuest -= ToggleSearchAreaOnMinimap;
    }

    public void ToggleSearchAreaOnMinimap(string id)
    {
        _quest = QuestManager.Instance.GetQuestById(id);
        int questIndex = GetIndexFromId(_quest.Info.id);

        ToggleSearchArea(questIndex);
    }

    private void ToggleSearchArea(int questIndex)
    {
        //Whale == 16
        //Ghost == 4
        //Bone == 1

        if (questIndex == 16)
        {
            _whaleSearchArea.SetActive(!_whaleSearchArea.activeInHierarchy);
        }
        else if (questIndex == 4)
        {
            _ghostSearchArea.SetActive(!_ghostSearchArea.activeInHierarchy);
        }
        else if (questIndex == 1)
        {
            _boneSearchArea.SetActive(!_boneSearchArea.activeInHierarchy);
        }
    }

    int GetIndexFromId(string id)
    {
        if (_idToIndex.ContainsKey(id))
        {
            return _idToIndex[id];
        }
        else
        {
            throw new KeyNotFoundException("ID not found: " + id);
        }
    }
}
