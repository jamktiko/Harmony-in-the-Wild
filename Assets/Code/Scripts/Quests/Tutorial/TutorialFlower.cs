using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialFlower : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuestSO;

    [SerializeField] private bool playerIsNear;
    private bool canBeCollected;

    private void Awake()
    {
        // check the initial status of the flower
        int currentTutorialQuestStepIndex = QuestManager.instance.GetQuestById(tutorialQuestSO.id).GetCurrentQuestStepIndex();

        if (currentTutorialQuestStepIndex == 1)
        {
            canBeCollected = true;
            Debug.Log("Collecting flower enabled.");
        }

        else if(currentTutorialQuestStepIndex > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += EnableCollection;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnAdvanceQuest += EnableCollection;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsNear && canBeCollected)
        {
            CollectFlowerQuestStep.instance.CollectFlower();
            Destroy(gameObject);
        }
    }

    private void EnableCollection(string questId)
    {
        if (!canBeCollected)
        {
            canBeCollected = true;
            Debug.Log("Collecting flower enabled.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }

        else
        {
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }
}
