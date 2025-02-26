using Cinemachine;
using UnityEngine;

public class PreventCameraMovementDuringDialogue : MonoBehaviour
{
    [SerializeField] private GameObject cameraMovement;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private void Start()
    {
        GameEventsManager.instance.dialogueEvents.OnStartDialogue += DisableMovement;
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += EnableMovement;
    }

    private void DisableMovement()
    {
        Debug.Log("Camera movement disabled!");

        if (cameraMovement == null)
        {
            Debug.Log("No camera movement attached to PreventCameraMovementDuringDialogue!");
        }

        else
        {
            cameraMovement.GetComponent<CameraMovement>().enabled = false;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }
    }

    private void EnableMovement()
    {
        Debug.Log("Camera movement enabled!");

        if (cameraMovement == null)
        {
            Debug.Log("No camera movement attached to PreventCameraMovementDuringDialogue!");
        }

        else
        {
            cameraMovement.GetComponent<CameraMovement>().enabled = true;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0.001f;
            freeLookCamera.m_XAxis.m_MaxSpeed = 0.1f;
        }
    }
}
