using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestMenuManager : MonoBehaviour
{
    [SerializeField] private Button _button; 
    private Transform _questMenuLayout;
    private List<Quest> _activeQuests;
    public static Quest TrackedQuest = null;
    private void OnEnable()
    {
        _questMenuLayout = GetComponent<Transform>();

        _activeQuests = new List<Quest>();
        _activeQuests = QuestManager.Instance.QuestMap.Where(x => x.Value.State == QuestState.CanStart || x.Value.State == QuestState.InProgress).Select(x => x.Value).OrderByDescending(x => x.State).ToList();
        foreach (Quest x in _activeQuests)
        {
            Button spawnableButton = _button;
            spawnableButton.name = x.Info.id;
            TMP_Text text = spawnableButton.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = x.Info.DisplayName;
            Instantiate(spawnableButton, _questMenuLayout);
        }
    }
    private void OnDisable()
    {
        foreach (Transform t in _questMenuLayout)
        {
            Destroy(t.gameObject);
        }
    }


}
