using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCheck : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    private void Start()
    {
        QuestState questState = QuestManager.instance.CheckQuestState(questSO.id);

        if(questState == QuestState.FINISHED)
        {
            gameObject.SetActive(false);
        }
    }
}
