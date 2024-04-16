using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCaveTrigger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private GameObject bearInstance;

    private void OnTriggerEnter(Collider other)
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex();

        if (other.gameObject.CompareTag("Trigger") && currentQuestStepIndex >= 3)
        {
            //ExitCaveQuest.instance.ExitCave(bearInstance);
            ExitCaveQuest.instance.ExitCave();
        }
    }
}
