using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //Note: does this need to be public? private and method to pass value instead

    void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame()&&isActive&& QuestManager.instance.CheckQuestState("Whale Diet").Equals(QuestState.IN_PROGRESS))
        {
            wasUsed = true;
            FindObjectOfType<CollectableQuestStep>().CollectableProgress();
            Debug.Log("object found!");
            Destroy(gameObject);
        }
        else if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive && QuestManager.instance.CheckQuestState("Whale Diet").Equals(QuestState.FINISHED))
        {
            wasUsed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isActive = false;
        }
    }
}
