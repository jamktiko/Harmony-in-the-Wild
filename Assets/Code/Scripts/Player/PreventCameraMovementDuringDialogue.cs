using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventCameraMovementDuringDialogue : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject freeLookCamera;

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnStartDialogue += DisableMovement;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += EnableMovement;
    }

    private void DisableMovement()
    {
        cameraMovement.enabled = false;
        freeLookCamera.SetActive(false);
    }

    private void EnableMovement()
    {
        cameraMovement.enabled = true;
        freeLookCamera.SetActive(true);
    }
}
