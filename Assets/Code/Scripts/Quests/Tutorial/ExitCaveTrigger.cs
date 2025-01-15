using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExitCaveTrigger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;

    private void OnTriggerEnter(Collider other)
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex();

        if (other.gameObject.CompareTag("Trigger") && currentQuestStepIndex >= 3)
        {
            ExitCaveQuest.instance.ExitCave();
            GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld_VS);            
        }
    }
}
