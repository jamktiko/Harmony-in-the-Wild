using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool used = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&isActive&& QuestManager.instance.CheckQuestState("Whale Diet").Equals(QuestState.IN_PROGRESS))
        {
            used = true;
            FindObjectOfType<CollectableQuestStep>().CollectableProgress();
            Debug.Log("object found!");
            Destroy(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("Whale Diet").Equals(QuestState.FINISHED))
        {
            used = true;
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
