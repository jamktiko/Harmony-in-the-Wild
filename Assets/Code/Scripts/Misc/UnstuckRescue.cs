using System.Collections.Generic;
using UnityEngine;

public class UnstuckRescue : MonoBehaviour
{
    private Vector3 _nearestRescuePoint;
    private List<Transform> _rescuePoints = new List<Transform>();
    private Transform _player;


    private void OnEnable()
    {
        CheckButtonVisibilityConditions();
    }

    private void GetNeededData()
    {
        _rescuePoints = UnstuckDataBank.Instance.GetRescuePoints();
        _player = FoxMovement.Instance.transform;
    }

    public void Unstuck()
    {
        _player = FoxMovement.Instance.transform;

        if (_player == null)
        {
            Debug.LogWarning("No player reference for Unstuck button!");
            return;
        }

        GameEventsManager.instance.UIEvents.UseUnstuckButton();
        _nearestRescuePoint = FindNearestRescuePoint();
        MovePlayerToSafeLocation();
    }

    private Vector3 FindNearestRescuePoint()
    {
        Vector3 newPosition = new Vector3();
        float smallestDistance = -1f;

        foreach (Transform rescuePoint in _rescuePoints)
        {
            float distanceToPlayer = Vector3.Distance(_player.position, rescuePoint.position);
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
        FoxMovement.Instance.gameObject.SetActive(false);
        _player.position = _nearestRescuePoint;
        FoxMovement.Instance.gameObject.SetActive(true);
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
