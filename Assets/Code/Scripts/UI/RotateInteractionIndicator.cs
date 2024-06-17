using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInteractionIndicator : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private GameObject playerObject;
    private bool playerIsNear;
    private Transform cameraOrientation;
    private Quaternion originalRotation;

    private GameObject interactionIndicatorUI;
    private Collider targetCollider;
    private Bounds bounds;
    private float fixedHeight;

    private Vector3 previousClosestPoint;
    private Vector3 closestPoint;
    private bool canUpdateUIPosition = false;

    private void Start()
    {
        interactionIndicatorUI = transform.GetChild(0).gameObject;
        targetCollider = GetComponent<Collider>();

        if (interactionIndicatorUI == null || targetCollider == null)
        {
            Debug.Log(gameObject.name + " should have an interaction indicator, but it is missing a necessary component! Interaction indicator shall be disabled.");
            enabled = false;
        }

        else
        {
            interactionIndicatorUI.SetActive(false);
            
            CalculateColliderBounds();
            SetFixedHeight();

            originalRotation = transform.rotation;
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
        if (canUpdateUIPosition)
        {
            UpdateInteractionIndicatorTargetPosition();
        }
        //interactionIndicatorUI.transform.position = closestPoint;
        
        if(previousClosestPoint != null)
        {
            //UpdateInterpolationRatio();

            interactionIndicatorUI.transform.position = Vector3.MoveTowards(interactionIndicatorUI.transform.position, closestPoint, moveSpeed * Time.deltaTime);

            /*Vector3 directionToPlayer = new Vector3(playerObject.transform.position.x - interactionIndicatorUI.transform.position.x, 0, playerObject.transform.position.z - interactionIndicatorUI.transform.position.z);

            if(directionToPlayer.sqrMagnitude > 0f)
            {
                Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
                interactionIndicatorUI.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0) * originalRotation;
            }*/

            transform.rotation = cameraOrientation.rotation * originalRotation;
        }
    }

    private void CalculateColliderBounds()
    {
        bounds = targetCollider.bounds;
    }

    private void SetFixedHeight()
    {
        fixedHeight = interactionIndicatorUI.transform.position.y;
    }

    private void UpdateInteractionIndicatorTargetPosition()
    {
        previousClosestPoint = closestPoint;

        closestPoint = bounds.ClosestPoint(playerObject.transform.position);
        closestPoint.y = fixedHeight;

        //elapsedFrames = 0;
        canUpdateUIPosition = false;

        Invoke(nameof(EnableUIPositionUpdate), 0.5f);
    }

    private void EnableUIPositionUpdate()
    {
        canUpdateUIPosition = true;
    }

    /*private void UpdateInterpolationRatio()
    {
        elapsedFrames += 0.5f;
        interpolationRatio = elapsedFrames / interpolationFrameCount;

        if(interpolationRatio >= 1)
        {
            canUpdateUIPosition = true;
        }
    }*/

    private void InitializeInteractionIndicatorPosition()
    {
        closestPoint = bounds.ClosestPoint(playerObject.transform.position);
        closestPoint.y = fixedHeight;

        interactionIndicatorUI.transform.position = closestPoint;
        //previousClosestPoint = closestPoint;

        //interpolationRatio = 1;
    }

    public void EnableInteractionIndicator(GameObject player, Transform orientation)
    {
        playerObject = player;
        playerIsNear = true;
        cameraOrientation = orientation;

        InitializeInteractionIndicatorPosition();

        interactionIndicatorUI.SetActive(true);
    }

    public void DisableInteractionIndicator()
    {
        playerIsNear = false;
        interactionIndicatorUI.SetActive(false);
    }
}
