using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ClosingWall : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float wallMovingSpeed;
    [SerializeField] private float wallMovingSpeedIncrease;
    [SerializeField] private float timeOnTargetSpot;

    [Header("Needed References")]
    [SerializeField] private Transform targetSpot;

    [Header("Debug")]
    [SerializeField] private bool isPlayerNear;

    // private variables
    private bool canMove = true;
    private Vector3 startSpot;
    private float timeToTargetSpot;
    private float elapsedTime;
    private Freezable freezable;
    private Collider coll;
    private float elapsedPercentage;
    private List<Vector3> targetPositions = new List<Vector3>();
    private Vector3 currentTargetPosition;

    private void Start()
    {
        startSpot = transform.position;
        freezable = GetComponent<Freezable>();
        coll = GetComponent<BoxCollider>();

        SetTimeToTarget();

        // initialize target position list
        targetPositions.Add(startSpot);
        targetPositions.Add(targetSpot.position);

        currentTargetPosition = targetSpot.position;

        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetWallPosition;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ResetAfterFinishedLap;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetWallPosition;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ResetAfterFinishedLap;
    }

    private void Update()
    {
        if (isPlayerNear && !freezable.isFrozen && canMove)
        {
            MoveWall();
        }

        // disable collider if the wall has been frozen, so it's not damaging the player when they walk past it
        if (freezable.isFrozen && coll.isTrigger)
        {
            //Debug.Log("Disabled collider");
            coll.isTrigger = false;
        }

        else if (!freezable.isFrozen && !coll.isTrigger)
        {
            //Debug.Log("Enabled collider");
            coll.isTrigger = true;
        }
    }

    private void MoveWall()
    {
        // update progress values
        elapsedTime += Time.deltaTime;
        elapsedPercentage = elapsedTime / timeToTargetSpot;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);

        transform.position = Vector3.Lerp(startSpot, currentTargetPosition, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            ChangeTargetPosition();
        }
    }

    private void ChangeTargetPosition()
    {
        // set position
        if (currentTargetPosition == targetPositions[0])
        {
            currentTargetPosition = targetPositions[1];
            startSpot = targetPositions[0];
        }

        else
        {
            currentTargetPosition = targetPositions[0];
            startSpot = targetPositions[1];
        }

        // set progress values
        elapsedTime = 0;
    }

    private void SetTimeToTarget()
    {
        float distanceToTarget = Vector3.Distance(startSpot, targetSpot.position);
        timeToTargetSpot = distanceToTarget / wallMovingSpeed;
    }

    public void PlayerGettingClose()
    {
        isPlayerNear = true;
    }

    private void ResetAfterFinishedLap()
    {
        ResetWallPosition();
        wallMovingSpeed += wallMovingSpeedIncrease;
    }

    private void ResetWallPosition()
    {
        transform.position = targetPositions[0];
        startSpot = targetPositions[0];
        currentTargetPosition = targetPositions[1];
        elapsedTime = 0;
        isPlayerNear = false;
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