using UnityEngine;
using UnityEngine.Serialization;

public class ToggleCinematicCamera : MonoBehaviour
{
    [FormerlySerializedAs("playerCamera")] [SerializeField] private GameObject _playerCamera;

    private void OnEnable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics += EnableCamera;
        GameEventsManager.instance.CinematicsEvents.OnEndCinematics += DisableCamera;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics -= EnableCamera;
        GameEventsManager.instance.CinematicsEvents.OnEndCinematics -= DisableCamera;
    }

    private void DisableCamera()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        _playerCamera.SetActive(true);
    }

    private void EnableCamera()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        _playerCamera.SetActive(false);
    }
}
