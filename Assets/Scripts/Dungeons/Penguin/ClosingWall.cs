using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingWall : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedIncrease;
    [SerializeField] private float timeOnTargetSpot;

    [Header("Needed References")]
    [SerializeField] private Transform targetSpot;

    [Header("Debug")]
    [SerializeField] private bool playerIsNear;

    // private variables
    private ClosingWallMovement currentMovement = ClosingWallMovement.Down;
    private bool canMove = true;
    private Vector3 startSpot;
    private float timeToTargetSpot;
    private float elapsedTime;
    private Freezable freezable;
    private Collider coll;
    private float elapsedPercentage;

    private void Start()
    {
        startSpot = transform.position;
        freezable = GetComponent<Freezable>();
        coll = GetComponent<BoxCollider>();

        SetTimeToTarget();

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
        if (playerIsNear && !freezable.isFreezed && canMove)
        {
            switch (currentMovement)
            {
                case ClosingWallMovement.Down:
                    elapsedTime += Time.deltaTime;
                    elapsedPercentage = elapsedTime / timeToTargetSpot;
                    elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
                    transform.position = Vector3.Lerp(startSpot, targetSpot.position, elapsedPercentage);

                    if(Vector3.Distance(transform.position, targetSpot.position) < 0.2f)
                    {
                        currentMovement = ClosingWallMovement.Paused;
                    }

                    break;

                case ClosingWallMovement.Up:
                    elapsedTime += Time.deltaTime;
                    elapsedPercentage = elapsedTime / timeToTargetSpot;
                    elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
                    transform.position = Vector3.Lerp(targetSpot.position, startSpot, elapsedPercentage);

                    if (Vector3.Distance(transform.position, startSpot) < 0.2f)
                    {
                        currentMovement = ClosingWallMovement.Paused;
                    }

                    break;

                case ClosingWallMovement.Paused:
                    canMove = false;
                    StartCoroutine(PauseMovement());
                    break;
            }
        }

        // disable collider if the 
        if (freezable.isFreezed && coll.isTrigger)
        {
            coll.isTrigger = false;
        }

        else if (!freezable.isFreezed && !coll.isTrigger)
        {
            coll.isTrigger = true;
        }
    }

    private void SetTimeToTarget()
    {
        float distanceToTarget = Vector3.Distance(startSpot, targetSpot.position);
        timeToTargetSpot = distanceToTarget / moveSpeed;
    }

    public void PlayerGettingClose()
    {
        playerIsNear = true;
    }

    private void ResetAfterFinishedLap()
    {
        ResetWallPosition();
        moveSpeed += moveSpeedIncrease;
    }

    private void ResetWallPosition()
    {
        if(transform.position != startSpot)
        {
            transform.position = startSpot;
            elapsedTime = 0;
            playerIsNear = false;
        }
    }

    private IEnumerator PauseMovement()
    {
        yield return new WaitForSeconds(timeOnTargetSpot);

        elapsedTime = 0;

        if (Vector3.Distance(transform.position, startSpot) < 0.2f)
        {
            currentMovement = ClosingWallMovement.Down;
        }

        else
        {
            currentMovement = ClosingWallMovement.Up;
        }

        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player was hit by a closing wall.");
            other.GetComponent<HitCounter>().TakeHit(true);
        }
    }
}

public enum ClosingWallMovement
{
    Down,
    Up,
    Paused
}
