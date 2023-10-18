using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWayPointPath : MonoBehaviour
{
    public Transform GetWayPoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;

        if(nextWaypointIndex >= transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }
}
