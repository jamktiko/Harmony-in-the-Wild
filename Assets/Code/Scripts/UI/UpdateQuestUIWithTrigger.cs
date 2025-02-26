using UnityEngine;
using UnityEngine.Serialization;

public class UpdateQuestUIWithTrigger : MonoBehaviour
{
    [FormerlySerializedAs("updatedQuestProgressUI")] [SerializeField] private string _updatedQuestProgressUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI(_updatedQuestProgressUI);
            Destroy(this);
        }
    }
}
