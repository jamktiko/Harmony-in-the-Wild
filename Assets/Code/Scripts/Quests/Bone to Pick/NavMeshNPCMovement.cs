using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavMeshNpcMovement : MonoBehaviour
{
    [FormerlySerializedAs("character")]
    [Header("Quest Config")]
    [SerializeField] private DialogueQuestNpCs _character;
    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;
    [FormerlySerializedAs("questStepIndexToAllowMovement")] [SerializeField] private int _questStepIndexToAllowMovement;

    [FormerlySerializedAs("maxDistanceToPlayer")]
    [Header("Movement Config")]
    [SerializeField] private float _maxDistanceToPlayer = 10f;
    [FormerlySerializedAs("destinations")] [SerializeField] private List<Transform> _destinations;

    [FormerlySerializedAs("animator")]
    [Header("Other Needed References")]
    [SerializeField] private Animator _animator;
    [FormerlySerializedAs("interactionIndicator")] [SerializeField] private GameObject _interactionIndicator;
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;

    private Vector3 _currentTargetDestination;
    private int _currentTargetDestinationIndex;
    private bool _canMove;
    private bool _playerIsNear;
    private NavMeshAgent _agent;
    private Coroutine _idleCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        CheckInitialSpawnPoint();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnStartMovingQuestNpc += EnableMovement;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnStartMovingQuestNpc -= EnableMovement;
    }

    private void Update()
    {
        if (_canMove && _playerIsNear)
        {
            _idleCoroutine = null;
            MoveNpc();
        }

        else if(_canMove && !_playerIsNear && _idleCoroutine == null)
        {
            _idleCoroutine = StartCoroutine(Idle());
        }

        CheckDistanceToPlayer();
    }

    private void MoveNpc()
    {
        _agent.SetDestination(_currentTargetDestination);

        if (Vector3.Distance(transform.position, _currentTargetDestination) < 1.5f)
        {
            SetNewDestination();
        }
    }

    private void EnableMovement(DialogueQuestNpCs questCharacterToMove)
    {
        if(_character == questCharacterToMove)
        {
            if (QuestManager.Instance.GetQuestById(_questSo.id).GetCurrentQuestStepIndex() == _questStepIndexToAllowMovement)
            {
                _canMove = true;

                if (_animator != null)
                {
                    _animator.SetTrigger("walk");
                }

                else
                {
                    Debug.LogError($"No animator attached to { gameObject.name }!");
                }

                if (_interactionIndicator != null)
                {
                    _interactionIndicator.SetActive(false);
                }

                else
                {
                    Debug.LogError($"No interaction indicator attached to { gameObject.name }!");
                }
            }
        }
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (_currentTargetDestinationIndex < _destinations.Count - 1)
        {
            _currentTargetDestinationIndex++;

            _currentTargetDestination = _destinations[_currentTargetDestinationIndex].position;
        }

        else
        {
            GameEventsManager.instance.QuestEvents.ReachTargetDestinationToCompleteQuestStep(_questSo.id);
            _canMove = false;

            if (_interactionIndicator != null)
            {
                _interactionIndicator.SetActive(true);
            }

            else
            {
                Debug.LogError($"No interaction indicator attached to { gameObject.name }!");
            }
        }
    }

    private void CheckInitialSpawnPoint()
    {
        if (QuestManager.Instance.CheckQuestState(_questSo.id) == QuestState.CanFinish)
        {
            transform.position = _destinations[_destinations.Count - 1].position;
        }

        else
        {
            _currentTargetDestination = _destinations[0].position;
        }
    }

    private void CheckDistanceToPlayer()
    {
        _playerIsNear = Vector3.Distance(transform.position, _player.position) <= _maxDistanceToPlayer;
    }

    private IEnumerator Idle()
    {
        // set animation to idle

        yield return new WaitUntil(() => _playerIsNear);

        // set animation to move
    }
}