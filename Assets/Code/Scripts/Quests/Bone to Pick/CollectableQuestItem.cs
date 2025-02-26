using UnityEngine;

public class CollectableQuestItem : MonoBehaviour
{
    [SerializeField] private QuestItem itemType;

    private bool playerIsNear;

    private void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && playerIsNear)
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (itemType != QuestItem.Default)
        {
            GameEventsManager.instance.questEvents.CollectQuestItem(itemType);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }
}
