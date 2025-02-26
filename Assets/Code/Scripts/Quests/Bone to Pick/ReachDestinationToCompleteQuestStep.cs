using UnityEngine;

public class ReachDestinationToCompleteQuestStep : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int targetQuestStepIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            CheckQuestProgress();
        }
    }

    private void CheckQuestProgress()
    {
        int currentQuestStepIndex = QuestManager.instance.GetQuestById(questSO.id).GetCurrentQuestStepIndex();

        if (currentQuestStepIndex == targetQuestStepIndex)
        {
            GameEventsManager.instance.questEvents.ReachTargetDestinationToCompleteQuestStep(questSO.id);
        }

        else
        {
            Debug.Log($"Trying to progress {questSO.id} by entering a trigger collider; current quest step index not matching.");
        }
    }
}
