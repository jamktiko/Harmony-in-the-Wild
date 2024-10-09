using Cinemachine;
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

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnStartDialogue -= DisableMovement;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= EnableMovement;
    }

    private void DisableMovement()
    {
        cameraMovement.enabled = false;
    }

    private void EnableMovement()
    {
        cameraMovement.enabled = true;
    }
}
