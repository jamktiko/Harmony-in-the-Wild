using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInteractionIndicator : MonoBehaviour
{
    private bool playerIsNear;
    private Transform cameraOrientation;

    private GameObject interactionIndicatorUI;

    private void Start()
    {
        interactionIndicatorUI = transform.GetChild(0).gameObject;

        if (interactionIndicatorUI == null)
        {
            Debug.Log(gameObject.name + " should have an interaction indicator, but it is missing a necessary component! Interaction indicator shall be disabled.");
            enabled = false;
        }

        else
        {
            interactionIndicatorUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (playerIsNear)
        {
            DisableInteractionIndicator();
        }
    }

    private void Update()
    {
        if (playerIsNear)
        {
            TargetInteractionIndicatorTowardsPlayer();
        }
    }
    
    private void TargetInteractionIndicatorTowardsPlayer()
    {
        Vector3 directionToPlayer = new Vector3(cameraOrientation.transform.position.x - interactionIndicatorUI.transform.position.x, 0, cameraOrientation.transform.position.z - interactionIndicatorUI.transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        interactionIndicatorUI.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    public void EnableInteractionIndicator(Transform orientation)
    {
        playerIsNear = true;
        cameraOrientation = orientation;

        interactionIndicatorUI.SetActive(true);
    }

    public void DisableInteractionIndicator()
    {
        playerIsNear = false;
        interactionIndicatorUI.SetActive(false);
    }
}
