using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelBossKeyCounter : MonoBehaviour
{
    [SerializeField] private GameObject finalDoor;
    [SerializeField] private FinishDungeonQuestStepWithTrigger finishDungeonScript;
    private int collectedKeys = 0;
    private int keysTotal = 5;

    public void CollectKey()
    {
        collectedKeys++;

        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Keys gathered " + collectedKeys + "/" + keysTotal);

        if (collectedKeys >= keysTotal)
        {
            GameEventsManager.instance.questEvents.ShowQuestUI("The Flying Squirrel", "Find the final door and complete the quest", "");
            finishDungeonScript.EnableInteraction();
            finalDoor.SetActive(true);
        }
    }
}
