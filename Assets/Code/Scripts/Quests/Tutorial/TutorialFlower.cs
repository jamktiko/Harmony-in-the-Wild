using UnityEngine;
using UnityEngine.Serialization;

public class TutorialFlower : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuestSO")] [SerializeField] private QuestScriptableObject _tutorialQuestSo;

    [FormerlySerializedAs("playerIsNear")] [SerializeField] private bool _playerIsNear;
    [FormerlySerializedAs("canBeCollected")] [SerializeField] private bool _canBeCollected;

    private void Awake()
    {
        Invoke(nameof(CheckCurrentQuestStep), 1f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += EnableCollection;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += EnableCollection;
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _playerIsNear && _canBeCollected)
        {
            CollectFlowerQuestStep.Instance.CollectFlower();
            Destroy(gameObject);
        }
    }

    private void EnableCollection(string questId)
    {
        if (!_canBeCollected)
        {
            _canBeCollected = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            _playerIsNear = true;
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
            _playerIsNear = false;
        }
    }

    private void CheckCurrentQuestStep()
    {
        // check the initial status of the flower
        int currentTutorialQuestStepIndex = QuestManager.Instance.GetQuestById(_tutorialQuestSo.id).GetCurrentQuestStepIndex();

        if (currentTutorialQuestStepIndex == 1)
        {
            _canBeCollected = true;
        }

        else if (currentTutorialQuestStepIndex > 1)
        {
            Destroy(gameObject);
        }
    }
}
