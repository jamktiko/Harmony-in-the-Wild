using UnityEngine;

public class ToggleCinematicCamera : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += EnableCamera;
        GameEventsManager.instance.cinematicsEvents.OnEndCinematics += DisableCamera;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= EnableCamera;
        GameEventsManager.instance.cinematicsEvents.OnEndCinematics -= DisableCamera;
    }

    private void DisableCamera()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        playerCamera.SetActive(true);
    }

    private void EnableCamera()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        playerCamera.SetActive(false);
    }
}
