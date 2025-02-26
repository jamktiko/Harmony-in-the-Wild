using UnityEngine;
using UnityEngine.Serialization;

public class FinishDungeonQuestStepWithTrigger : MonoBehaviour
{
    [FormerlySerializedAs("dungeonQuest")] [SerializeField] private QuestScriptableObject _dungeonQuest;
    [FormerlySerializedAs("enableInteractionFromStart")]
    [Tooltip("If this box is ticked, you can immediately interact with the object without any other conditions as prerequisities.")]
    [SerializeField] private bool _enableInteractionFromStart;

    [FormerlySerializedAs("isLearningStage")]
    [Header("For Learning Stages Only")]
    [SerializeField] private bool _isLearningStage;
    [FormerlySerializedAs("nextScene")] [SerializeField] private SceneManagerHelper.Scene _nextScene;

    private bool _canFinishQuest;
    private string _questId;

    private void Start()
    {
        _questId = _dungeonQuest.id;

        if (_enableInteractionFromStart)
        {
            _canFinishQuest = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canFinishQuest)
        {
            Debug.LogWarning("Interaction with the transition object has not been enabled yet. Check if enabling it" +
                "has been called from other scripts handling the enabling conditions (for example, collecting items) " +
                "or that Enable Interaction From Start has been ticked.");
            return;
        }

        if (_canFinishQuest && other.CompareTag("Trigger"))
        {
            GameEventsManager.instance.QuestEvents.AdvanceDungeonQuest(_questId);

            if (_isLearningStage)
            {
                Invoke(nameof(ChangeScene), 0.15f);
            }
        }
    }

    private void ChangeScene()
    {
        SceneManagerHelper.LoadScene(_nextScene);
    }

    public void EnableInteraction()
    {
        _canFinishQuest = true;
    }
}
