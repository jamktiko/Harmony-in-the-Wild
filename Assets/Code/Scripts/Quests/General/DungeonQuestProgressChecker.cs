using UnityEngine;
using UnityEngine.Serialization;

public class DungeonQuestProgressChecker : MonoBehaviour
{
    [FormerlySerializedAs("quest")] [SerializeField] private QuestScriptableObject _quest;
    [FormerlySerializedAs("ability")] [SerializeField] private Abilities _ability;

    private QuestState _currentState;

    private void Start()
    {
        _currentState = QuestManager.Instance.CheckQuestState(_quest.id);

        CheckForAbilityLock();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.CinematicsEvents.OnEndCinematics += MarkQuestAsFinished;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.CinematicsEvents.OnEndCinematics -= MarkQuestAsFinished;
    }

    private void MarkQuestAsFinished()
    {
        // if quest is ready to be finished, mark it as finished
        if (_currentState == QuestState.CanFinish)
        {
            GameEventsManager.instance.QuestEvents.FinishQuest(_quest.id);
        }
    }

    private void CheckForAbilityLock()
    {
        // if quest is still in progress, lock the ability so the player cannot yet use it in Overworld
        if (_currentState == QuestState.Finished || _currentState == QuestState.CanFinish)
        {
            AbilityManager.Instance.UnlockAbility(_ability);
            return;
        }

        else
        {
            AbilityManager.Instance.LockAbility(_ability);
        }
    }
}