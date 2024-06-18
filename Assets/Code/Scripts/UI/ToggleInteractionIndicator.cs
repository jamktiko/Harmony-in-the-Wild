using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ToggleInteractionIndicator : MonoBehaviour
{
    private RotateInteractionIndicator rotationComponent;

    private void Start()
    {
        rotationComponent = GetComponentInChildren<RotateInteractionIndicator>();

        if(rotationComponent == null)
        {
            Debug.Log(gameObject.name + " doesn't have RotateInteractionIndicator component which is mandatory for interactable objects. Interaction will be disabled.");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            Debug.Log("Player detected");

            // locate the orientation object
            Transform orientation = other.transform.parent.Find("Orientation");
            GameObject camera = other.transform.parent.Find("FreeLook Camera").gameObject;

            if(orientation != null)
            {
                rotationComponent.EnableInteractionIndicator(other.gameObject, orientation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            rotationComponent.DisableInteractionIndicator();
        }
    }
}
