using System.Collections.Generic;
using UnityEngine;

public class UnstuckRescue : MonoBehaviour
{
    private Vector3 nearestRescuePoint;
    private List<Transform> rescuePoints = new List<Transform>();
    private Transform player;


    private void OnEnable()
    {
        CheckButtonVisibilityConditions();
    }

    private void GetNeededData()
    {
        rescuePoints = UnstuckDataBank.instance.GetRescuePoints();
        player = FoxMovement.instance.transform;
    }

    public void Unstuck()
    {
        player = FoxMovement.instance.transform;

        if (player == null)
        {
            Debug.LogWarning("No player reference for Unstuck button!");
            return;
        }

        GameEventsManager.instance.uiEvents.UseUnstuckButton();
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

            if (distanceToPlayer < smallestDistance || smallestDistance == -1f)
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

    private void CheckButtonVisibilityConditions()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GetNeededData();
        }
    }
}
