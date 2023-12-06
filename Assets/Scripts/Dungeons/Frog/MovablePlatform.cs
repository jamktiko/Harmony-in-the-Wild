using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] private PlatformWayPointPath wayPointPath;
    [SerializeField] private float moveSpeed;

    private int targetWaypointIndex;
    private Transform previousWaypoint;
    private Transform targetWayPoint;

    private float timeToWaypoint;
    private float elapsedTime;

    private void Start()
    {
        TargetNextWaypoint();
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsetPercentage = elapsedTime / timeToWaypoint;
        //elapsetPercentage = Mathf.SmoothStep(0, 1, elapsetPercentage);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWayPoint.position, elapsetPercentage);

        if(elapsetPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = wayPointPath.GetWayPoint(targetWaypointIndex);
        targetWaypointIndex = wayPointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWayPoint = wayPointPath.GetWayPoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWayPoint = Vector3.Distance(previousWaypoint.position, targetWayPoint.position);
        timeToWaypoint = distanceToWayPoint / moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            other.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            other.transform.parent.SetParent(null);
        }
    }
}