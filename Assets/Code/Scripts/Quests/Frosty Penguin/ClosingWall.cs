using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ClosingWall : MonoBehaviour
{
    [FormerlySerializedAs("wallMovingSpeed")]
    [Header("Config")]
    [SerializeField] private float _wallMovingSpeed;
    [FormerlySerializedAs("wallMovingSpeedIncrease")] [SerializeField] private float _wallMovingSpeedIncrease;
    [FormerlySerializedAs("timeOnTargetSpot")] [SerializeField] private float _timeOnTargetSpot;

    [FormerlySerializedAs("targetSpot")]
    [Header("Needed References")]
    [SerializeField] private Transform _targetSpot;

    [FormerlySerializedAs("isPlayerNear")]
    [Header("Debug")]
    [SerializeField] private bool _isPlayerNear;

    // private variables
    private bool _canMove = true;
    private Vector3 _startSpot;
    private float _timeToTargetSpot;
    private float _elapsedTime;
    private Freezable _freezable;
    private Collider _coll;
    private float _elapsedPercentage;
    private List<Vector3> _targetPositions = new List<Vector3>();
    private Vector3 _currentTargetPosition;

    private void Start()
    {
        _startSpot = transform.position;
        _freezable = GetComponent<Freezable>();
        _coll = GetComponent<BoxCollider>();

        SetTimeToTarget();

        // initialize target position list
        _targetPositions.Add(_startSpot);
        _targetPositions.Add(_targetSpot.position);

        _currentTargetPosition = _targetSpot.position;

        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted += ResetWallPosition;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += ResetAfterFinishedLap;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted -= ResetWallPosition;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= ResetAfterFinishedLap;
    }

    private void Update()
    {
        if (_isPlayerNear && !_freezable.IsFrozen && _canMove)
        {
            MoveWall();
        }

        // disable collider if the wall has been frozen, so it's not damaging the player when they walk past it
        if (_freezable.IsFrozen && _coll.isTrigger)
        {
            //Debug.Log("Disabled collider");
            _coll.isTrigger = false;
        }

        else if (!_freezable.IsFrozen && !_coll.isTrigger)
        {
            //Debug.Log("Enabled collider");
            _coll.isTrigger = true;
        }
    }

    private void MoveWall()
    {
        // update progress values
        _elapsedTime += Time.deltaTime;
        _elapsedPercentage = _elapsedTime / _timeToTargetSpot;
        _elapsedPercentage = Mathf.SmoothStep(0, 1, _elapsedPercentage);

        transform.position = Vector3.Lerp(_startSpot, _currentTargetPosition, _elapsedPercentage);

        if (_elapsedPercentage >= 1)
        {
            ChangeTargetPosition();
        }
    }

    private void ChangeTargetPosition()
    {
        // set target position
        if (_currentTargetPosition == _targetPositions[0])
        {
            // go down
            _currentTargetPosition = _targetPositions[1];
            _startSpot = _targetPositions[0];

            // enable collider
            _coll.enabled = true;
        }

        else
        {
            // go up
            _currentTargetPosition = _targetPositions[0];
            _startSpot = _targetPositions[1];

            // disable collider
            _coll.enabled = false;
        }

        // set progress values
        _elapsedTime = 0;
    }

    private void SetTimeToTarget()
    {
        float distanceToTarget = Vector3.Distance(_startSpot, _targetSpot.position);
        _timeToTargetSpot = distanceToTarget / _wallMovingSpeed;
    }

    public void PlayerGettingClose()
    {
        _isPlayerNear = true;
    }

    private void ResetAfterFinishedLap()
    {
        ResetWallPosition();
        _wallMovingSpeed += _wallMovingSpeedIncrease;
    }

    private void ResetWallPosition()
    {
        transform.position = _targetPositions[0];
        _startSpot = _targetPositions[0];
        _currentTargetPosition = _targetPositions[1];
        _elapsedTime = 0;
        _isPlayerNear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Debug.Log("Player was hit by a closing wall.");
            other.GetComponentInParent<HitCounter>().TakeHit(true);
        }
    }
}