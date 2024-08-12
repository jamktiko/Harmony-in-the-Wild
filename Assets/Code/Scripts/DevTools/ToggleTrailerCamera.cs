using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTrailerCamera : MonoBehaviour
{
    [SerializeField] private GameObject trailerCamera;
    [SerializeField] private List<GameObject> playerCameras;
    [SerializeField] private FoxMovement foxmove;

    private bool trailerCameraOn;

    private void Update()
    {
        if (PlayerInputHandler.instance.DebugTrailerCameraToggle.WasPressedThisFrame())
        {
            ToggleCameras();
        }
    }

    private void ToggleCameras()
    {
        trailerCameraOn = !trailerCameraOn;

        if (trailerCameraOn)
        {
            foreach(GameObject camera in playerCameras)
            {
                camera.SetActive(false);
            }

            foxmove.enabled = false;
            trailerCamera.SetActive(true);
        }

        else
        {
            foreach (GameObject camera in playerCameras)
            {
                camera.SetActive(true);
            }

            foxmove.enabled = true;
            trailerCamera.SetActive(false);
        }
    }
}
