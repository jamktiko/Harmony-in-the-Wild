using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingInPenguin : MonoBehaviour
{
    private FoxMovement foxMovement;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameObject camera;

    private void Start()
    {
        foxMovement = GetComponent<FoxMovement>();
        PenguinRaceManager.instance.penguinDungeonEvents.onTimeRanOut += DisableMovement;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onTimeRanOut -= DisableMovement;
    }

    private void DisableMovement()
    {
        foxMovement.enabled = false;
        cameraMovement.enabled = false;
        camera.gameObject.SetActive(false);
    }
}
