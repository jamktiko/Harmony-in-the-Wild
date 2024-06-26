using UnityEngine;

public class QuestUISetter : MonoBehaviour
{
    [SerializeField] private int questIndex;
    [SerializeField] private bool canGoToSecondObjective;

    private void Start()
    {
        //GameEventsManager.instance.questEvents.ShowQuestUI(questIndex);

        if (canGoToSecondObjective)
        {
            //GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
        }
    }
}
