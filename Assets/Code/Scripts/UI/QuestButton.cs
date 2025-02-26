using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestButton : MonoBehaviour
{
    [FormerlySerializedAs("activeQuestUI")] [SerializeField] ActiveQuestUI _activeQuestUI;
    private void Awake()
    {
        //In case there are several activeQuestUI objects in the scene, we need to find the right one
        var activeQuestUIList = FindObjectsOfType<ActiveQuestUI>(true).ToList();
        _activeQuestUI = activeQuestUIList.Count > 1 ? activeQuestUIList[1] : activeQuestUIList[0];

        if (QuestMenuManager.TrackedQuest != null && !_activeQuestUI.gameObject.activeSelf)
        {
            _activeQuestUI.gameObject.SetActive(true);
            _activeQuestUI.UpdateQuestMenuUI();
        }
    }
    public void ChooseQuest()
    {
        string name = gameObject.name;
        Quest thisQuest;
        var questList = QuestManager.Instance.QuestMap.Where(x => name.Contains(x.Value.Info.DisplayName)).Select(x => x.Value).ToList();
        thisQuest = questList.FirstOrDefault();

        QuestMenuManager.TrackedQuest = thisQuest;

        _activeQuestUI.gameObject.SetActive(true);
        _activeQuestUI.UpdateQuestMenuUI();
    }
}
