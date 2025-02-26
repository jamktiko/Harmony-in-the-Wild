using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleTrailerCamera : MonoBehaviour
{
#if DEBUG
    [FormerlySerializedAs("trailerCamera")] [SerializeField] private GameObject _trailerCamera;
    [FormerlySerializedAs("playerCameras")] [SerializeField] private List<GameObject> _playerCameras;
    [FormerlySerializedAs("foxmove")] [SerializeField] private FoxMovement _foxmove;

    private bool _trailerCameraOn;

    private void Update()
    {
        if (PlayerInputHandler.Instance.DebugTrailerCameraToggle.WasPressedThisFrame())
        {
            ToggleCameras();
        }
    }

    private void ToggleCameras()
    {
        _trailerCameraOn = !_trailerCameraOn;

        if (_trailerCameraOn)
        {
            foreach (GameObject camera in _playerCameras)
            {
                camera.SetActive(false);
            }

            _foxmove.enabled = false;
            _trailerCamera.SetActive(true);
        }

        else
        {
            foreach (GameObject camera in _playerCameras)
            {
                camera.SetActive(true);
            }

            _foxmove.enabled = true;
            _trailerCamera.SetActive(false);
        }
    }
#endif
}
