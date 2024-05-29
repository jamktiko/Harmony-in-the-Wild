using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCinematicCamera : MonoBehaviour
{
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
    }

    private void EnableCamera()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
