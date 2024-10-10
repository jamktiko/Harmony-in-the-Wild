using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PreventCameraMovementDuringDialogue : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private void Start()
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
        Debug.Log("Camera movement disabled!");
        cameraMovement.enabled = false;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
    }

    private void EnableMovement()
    {
        Debug.Log("Camera movement enabled!");
        cameraMovement.enabled = true;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0.001f;
        freeLookCamera.m_XAxis.m_MaxSpeed = 0.1f;
    }
}
