using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorialQuest : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameEventsManager.instance.questEvents.StartQuest(questSO.id);
        }
    }
}
