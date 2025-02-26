using UnityEngine;
using UnityEngine.Serialization;

public class SquirrelBossKeyCounter : MonoBehaviour
{
    [FormerlySerializedAs("finalDoor")] [SerializeField] private GameObject _finalDoor;
    [FormerlySerializedAs("finishDungeonScript")] [SerializeField] private FinishDungeonQuestStepWithTrigger _finishDungeonScript;
    private int _collectedKeys = 0;
    private int _keysTotal = 5;

    public void CollectKey()
    {
        _collectedKeys++;

        GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI("Keys gathered " + _collectedKeys + "/" + _keysTotal);

        if (_collectedKeys >= _keysTotal)
        {
            GameEventsManager.instance.QuestEvents.ShowQuestUI("The Flying Squirrel", "Find the final door and complete the quest", "");
            _finishDungeonScript.EnableInteraction();
            _finalDoor.SetActive(true);
        }
    }
}
