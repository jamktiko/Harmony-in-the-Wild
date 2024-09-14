using UnityEngine;

public class Interactable_BoneToPick : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //Note: does this need to be public? private and method to pass value instead

    void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive && QuestManager.instance.CheckQuestState("BoneToPick").Equals(QuestState.IN_PROGRESS))
        {
            wasUsed = true;
            FindObjectOfType<CollectableQuestStep_BoneToPick>().CollectableProgress();
            Debug.Log("object found!");
            Destroy(gameObject);
        }
        else if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive && QuestManager.instance.CheckQuestState("BoneToPick").Equals(QuestState.FINISHED))
        {
            wasUsed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }
}
