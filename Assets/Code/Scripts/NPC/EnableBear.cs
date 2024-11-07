using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBear : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;

    private void Start()
    {
        QuestState state = QuestManager.instance.CheckQuestState(tutorialQuest.id);

        if(state != QuestState.FINISHED)
        {
            gameObject.SetActive(false);
        }
    }
}
