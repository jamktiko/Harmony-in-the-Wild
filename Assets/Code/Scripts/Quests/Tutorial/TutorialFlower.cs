using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialFlower : MonoBehaviour
{
    private bool playerIsNear;
    private bool canBeCollected;

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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = true;
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
