using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUISetter : MonoBehaviour
{
    [SerializeField] private int questIndex;
    [SerializeField] private bool goToSecondObjective;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(questIndex);

        if (goToSecondObjective)
        {
            GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
        }
    }
}
