using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestNpcMovement : MonoBehaviour
{
    [FormerlySerializedAs("questSO")]
    [Header("Quest Config")]
    [SerializeField] private QuestScriptableObject _questSo;
    [FormerlySerializedAs("character")] [SerializeField] private DialogueQuestNpCs _character;

    [FormerlySerializedAs("moveSpeed")]
    [Header("Movement Config")]
    [SerializeField] private float _moveSpeed = 2f;
    [FormerlySerializedAs("rotationSpeed")] [SerializeField] private float _rotationSpeed = 1.5f;
    [FormerlySerializedAs("maxDistanceToPlayer")] [SerializeField] private float _maxDistanceToPlayer = 10f;

    [FormerlySerializedAs("destinations")]
    [Header("Nav Mesh Movement Config")]

    [Header("Needed References")]
    [SerializeField] private List<Transform> _destinations;
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;
    //[SerializeField] private Animator animator;

    private float _defaultSpeed;
    private Vector3 _currentDestination;
    private int _currentDestinationIndex;
    private bool _playerIsNear;
    private Coroutine _idleCoroutine;

    private void Start()
    {
        if (QuestManager.Instance.CheckQuestState(_questSo.id) == QuestState.CanFinish)
        {
            transform.position = _destinations[_destinations.Count - 1].position;
        }

        else
        {
            _defaultSpeed = _moveSpeed;
            _currentDestination = _destinations[0].position;
        }
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
        _playerIsNear = Vector3.Distance(transform.position, _player.position) <= _maxDistanceToPlayer;
    }

    private void EnableMovement(DialogueQuestNpCs characterToMove)
    {
        if (_character == characterToMove)
        {
            StartCoroutine(WalkToDestination());

            Debug.Log("Start quest NPC movement...");
        }
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (_currentDestinationIndex < _destinations.Count - 1)
        {
            _currentDestinationIndex++;

            _currentDestination = _destinations[_currentDestinationIndex].position;

            StartCoroutine(WalkToDestination());
        }

        else
        {
            GameEventsManager.instance.QuestEvents.ReachWhaleDestination();
        }
    }

    private IEnumerator WalkToDestination()
    {
        // make sure there is no overlapping idle coroutines going
        if (_idleCoroutine != null)
        {
            _idleCoroutine = null;
        }

        yield return new WaitUntil(() => _playerIsNear);

        bool nearDestination = false;

        while (!nearDestination && _playerIsNear)
        {
            // move towards the target location
            transform.position = Vector3.MoveTowards(transform.position, _currentDestination, _moveSpeed * Time.deltaTime);

            // rotate towards the target location
            Vector3 direction = (_currentDestination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

            if (Vector3.Distance(transform.position, _currentDestination) < 1.5f)
            {
                nearDestination = true;
            }

            yield return null;
        }

        if (_playerIsNear)
        {
            SetNewDestination();
        }

        else
        {
            _idleCoroutine = StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle()
    {
        yield return new WaitUntil(() => _playerIsNear);

        StartCoroutine(WalkToDestination());
    }
}
