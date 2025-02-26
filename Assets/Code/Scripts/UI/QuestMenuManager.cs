using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenuManager : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Transform questMenuLayout;
    [SerializeField] List<Quest> activeQuests;
    public static Quest trackedQuest = null;
    private void OnEnable()
    {
        questMenuLayout = GetComponent<Transform>();

        activeQuests = new List<Quest>();
        activeQuests = QuestManager.instance.questMap.Where(x => x.Value.state == QuestState.CAN_START || x.Value.state == QuestState.IN_PROGRESS).Select(x => x.Value).OrderByDescending(x => x.state).ToList();
        foreach (Quest x in activeQuests)
        {
            Button spawnableButton = button;
            spawnableButton.name = x.info.id;
            TMP_Text text = spawnableButton.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = x.info.displayName;
            Instantiate(spawnableButton, questMenuLayout);
        }
    }
    private void OnDisable()
    {
        foreach (Transform t in questMenuLayout)
        {
            Destroy(t.gameObject);
        }
    }


}
