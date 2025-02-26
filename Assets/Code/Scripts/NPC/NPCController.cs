using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NpcController : MonoBehaviour
{
    [FormerlySerializedAs("minDuration")]
    [Header("Idle Duration Config")]
    [SerializeField] private float _minDuration = 2f;
    [FormerlySerializedAs("maxDuration")] [SerializeField] private float _maxDuration = 10f;

    [FormerlySerializedAs("destinations")]
    [Header("Needed References")]
    [SerializeField] private List<Transform> _destinations;
    [FormerlySerializedAs("animator")] [SerializeField] private Animator _animator;

    private NavMeshAgent _agent;
    private Vector3 _currentDestination;
    private int _currentDestinationIndex;
    private float _defaultSpeed;
    private bool _playerIsNear;
    private Coroutine _idleCoroutine;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentDestination = _destinations[0].position;
        _defaultSpeed = _agent.speed;

        StartCoroutine(WalkToDestination());
    }

    private void SetNewDestination()
    {
        // if the list contains a new destination, set it is current destination
        // otherwise use the first destination as a target to start a new loop
        if (_currentDestinationIndex < _destinations.Count - 1)
        {
            _currentDestinationIndex++;
        }

        else
        {
            _currentDestinationIndex = 0;
        }

        _currentDestination = _destinations[_currentDestinationIndex].position;
    }

    private IEnumerator WalkToDestination()
    {
        // make sure there is no overlapping idle coroutines going
        if (_idleCoroutine != null)
        {
            _idleCoroutine = null;
        }

        bool nearDestination = false;

        SetNewDestination();

        SetTurnAnimation();

        while (!nearDestination && !_playerIsNear)
        {
            _agent.SetDestination(_currentDestination);

            if (Vector3.Distance(transform.position, _currentDestination) < 1.5f)
            {
                nearDestination = true;
            }

            yield return null;
        }

        _idleCoroutine = StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {
        _agent.speed = 0;
        _animator.SetTrigger("idle");

        yield return new WaitForSeconds(Random.Range(_minDuration, _maxDuration));

        //yield return new WaitUntil(playerIsNear == false);

        if (!_playerIsNear)
        {
            _agent.speed = _defaultSpeed;
            StartCoroutine(WalkToDestination());
        }
    }

    private void SetTurnAnimation()
    {
        float direction = transform.position.x - _currentDestination.x;

        _animator.SetTrigger("walk");
        _animator.SetFloat("turn", direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsNear = false;

            if (_idleCoroutine == null)
            {
                StartCoroutine(Idle());
            }
        }
    }
}
