using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstuckRescue : MonoBehaviour
{
    private Vector3 nearestRescuePoint;
    private List<Transform> rescuePoints = new List<Transform>();
    private Transform player;

    private void Start()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            gameObject.SetActive(false);
        }

        else
        {
            GetNeededData();
        }
    }

    private void GetNeededData()
    {
        rescuePoints = UnstuckDataBank.instance.GetRescuePoints();
        player = UnstuckDataBank.instance.GetPlayer();
    }

    public void Unstuck()
    {
        player = UnstuckDataBank.instance.GetPlayer();

        nearestRescuePoint = FindNearestRescuePoint();
        MovePlayerToSafeLocation();
    }

    private Vector3 FindNearestRescuePoint()
    {
        Vector3 newPosition = new Vector3();
        float smallestDistance = -1f;

        foreach (Transform rescuePoint in rescuePoints)
        {
            float distanceToPlayer = Vector3.Distance(player.position, rescuePoint.position);
            Debug.Log("Distance to player: " + distanceToPlayer);

            if(distanceToPlayer < smallestDistance || smallestDistance == -1f)
            {
                smallestDistance = distanceToPlayer;
                newPosition = rescuePoint.position;
            }
        }

        return newPosition;
    }

    private void MovePlayerToSafeLocation()
    {
        FoxMovement.instance.gameObject.SetActive(false);
        player.position = nearestRescuePoint;
        FoxMovement.instance.gameObject.SetActive(true);
    }
}
