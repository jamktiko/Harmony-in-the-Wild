using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraSummon : MonoBehaviour
{
    [SerializeField] GameObject cinCam;
    [SerializeField]GameObject instance;
    private void Update()
    {
        //StartCinematicCamera();
    }
    void StartCinematicCamera() 
    {
        if (PlayerInputHandler.instance.CinematicCamera.WasPressedThisFrame() && instance == null)
        {
            instance = Instantiate(cinCam, transform.position, transform.rotation);
        }
        else if (PlayerInputHandler.instance.CinematicCamera.WasPressedThisFrame() && instance != null) 
        {
            DestroyImmediate(instance);
            instance = null;
        }
    }
}
