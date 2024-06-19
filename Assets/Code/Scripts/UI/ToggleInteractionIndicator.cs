using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ToggleInteractionIndicator : MonoBehaviour
{
    private RotateInteractionIndicator rotationComponent;

    private void Start()
    {
        rotationComponent = GetComponent<RotateInteractionIndicator>();

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
            // locate the player camera and enable interaction indicator
            GameObject camera = other.transform.parent.Find("FreeLook Camera").gameObject;

            if(camera != null)
            {
                rotationComponent.EnableInteractionIndicator(camera.transform);
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
