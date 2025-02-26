using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PreventCameraMovementDuringDialogue : MonoBehaviour
{
    [FormerlySerializedAs("cameraMovement")] [SerializeField] private GameObject _cameraMovement;
    [FormerlySerializedAs("freeLookCamera")] [SerializeField] private CinemachineFreeLook _freeLookCamera;

    private void Start()
    {
        GameEventsManager.instance.DialogueEvents.OnStartDialogue += DisableMovement;
        GameEventsManager.instance.DialogueEvents.OnEndDialogue += EnableMovement;
    }

    private void DisableMovement()
    {
        Debug.Log("Camera movement disabled!");

        if (_cameraMovement == null)
        {
            Debug.Log("No camera movement attached to PreventCameraMovementDuringDialogue!");
        }

        else
        {
            _cameraMovement.GetComponent<CameraMovement>().enabled = false;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 0;
            _freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        }
    }

    private void EnableMovement()
    {
        Debug.Log("Camera movement enabled!");

        if (_cameraMovement == null)
        {
            Debug.Log("No camera movement attached to PreventCameraMovementDuringDialogue!");
        }

        else
        {
            _cameraMovement.GetComponent<CameraMovement>().enabled = true;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 0.001f;
            _freeLookCamera.m_XAxis.m_MaxSpeed = 0.1f;
        }
    }
}
