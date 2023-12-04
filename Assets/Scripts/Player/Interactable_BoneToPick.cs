using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_BoneToPick : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool used = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("BoneToPick").Equals(QuestState.IN_PROGRESS))
        {
            used = true;
            FindObjectOfType<CollectableQuestStep_BoneToPick>().CollectableProgress();
            Debug.Log("object found!");
            Destroy(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("BoneToPick").Equals(QuestState.FINISHED))
        {
            used = true;
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
