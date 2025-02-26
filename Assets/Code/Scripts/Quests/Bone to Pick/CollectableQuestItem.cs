using UnityEngine;
using UnityEngine.Serialization;

public class CollectableQuestItem : MonoBehaviour
{
    [FormerlySerializedAs("itemType")] [SerializeField] private QuestItem _itemType;

    private bool _playerIsNear;

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear)
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        if (_itemType != QuestItem.Default)
        {
            GameEventsManager.instance.QuestEvents.CollectQuestItem(_itemType);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            _playerIsNear = false;
        }
    }
}
