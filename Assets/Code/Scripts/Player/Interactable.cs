using UnityEngine;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    [FormerlySerializedAs("isActive")] [SerializeField] bool _isActive = false;
    [FormerlySerializedAs("wasUsed")] [SerializeField] public bool WasUsed = false; //Note: does this need to be public? private and method to pass value instead

    private int _index;

    private void Start()
    {
        _index = transform.GetSiblingIndex();
    }

    void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive && QuestManager.Instance.CheckQuestState("Whale Diet").Equals(QuestState.InProgress))
        {
            WasUsed = true;
            FindObjectOfType<CollectableQuestStep>().CollectableProgress(_index);
            //Debug.Log("object found!");
            Destroy(gameObject);
        }
        //else if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive && QuestManager.instance.CheckQuestState("Whale Diet").Equals(QuestState.FINISHED))
        //{
        //    wasUsed = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _isActive = false;
        }
    }
}
