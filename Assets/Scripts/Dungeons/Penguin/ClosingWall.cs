using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingWall : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;

    [Header("Needed References")]
    [SerializeField] private Transform targetSpot;

    [Header("Debug")]
    [SerializeField] private bool playerIsNear;

    // private variables
    private Vector3 startSpot;
    private float timeToTargetSpot;
    private float elapsedTime;
    private Freezable freezable;

    private void Start()
    {
        startSpot = transform.position;
        freezable = GetComponent<Freezable>();

        SetTimeToTarget();

        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetWallPosition;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ResetWallPosition;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetWallPosition;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ResetWallPosition;
    }

    private void Update()
    {
        if (playerIsNear && !freezable.isFreezed)
        {
            elapsedTime += Time.deltaTime;

            float elapsetPercentage = elapsedTime / timeToTargetSpot;
            elapsetPercentage = Mathf.SmoothStep(0, 1, elapsetPercentage);
            transform.position = Vector3.Lerp(startSpot, targetSpot.position, elapsetPercentage);
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

    private void ResetWallPosition()
    {
        if(transform.position != startSpot)
        {
            transform.position = startSpot;
            elapsedTime = 0;
            playerIsNear = false;
        }
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
