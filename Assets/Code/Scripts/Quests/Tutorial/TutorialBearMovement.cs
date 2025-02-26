using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class TutorialBearMovement : MonoBehaviour
{
    [FormerlySerializedAs("targetTransform")]
    [Header("Movement Config")]
    [SerializeField] private Transform _targetTransform;

    [FormerlySerializedAs("tutorialQuestSO")]
    [Header("Other Needed References")]
    [SerializeField] private QuestScriptableObject _tutorialQuestSo;
    [FormerlySerializedAs("animator")] [SerializeField] private Animator _animator;
    [FormerlySerializedAs("interactionIndicator")] [SerializeField] private GameObject _interactionIndicator;

    private bool _canMove;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += CheckMovementActivation;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest -= CheckMovementActivation;
    }

    private void Update()
    {
        if (_canMove && _targetTransform != null)
        {
            _agent.SetDestination(_targetTransform.position);

            if (Vector3.Distance(transform.position, _targetTransform.position) < 1.5f)
            {
                gameObject.SetActive(false);
                _canMove = false;
            }
        }
    }

    private void CheckMovementActivation(string questId)
    {
        if (questId == _tutorialQuestSo.id)
        {
            Invoke(nameof(EnableMovement), 0.5f);
        }
    }

    private void EnableMovement()
    {
        if (QuestManager.Instance.GetQuestById(_tutorialQuestSo.id).GetCurrentQuestStepIndex() == 3)
        {
            _canMove = true;
            _animator.SetTrigger("walk");

            if (_interactionIndicator != null)
            {
                _interactionIndicator.SetActive(false);
            }

            else
            {
                Debug.LogError("No interaction indicator attached to Tutorial Bear!");
            }
        }
    }
}
