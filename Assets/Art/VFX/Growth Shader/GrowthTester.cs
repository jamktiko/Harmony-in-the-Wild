using UnityEngine;

/* Test script to show how the GrowthController and CinematicCameraRotator work together.
 Replace the key press check with your own trigger mechanism.
 Assign the object to enable, GrowthController, and CinematicCameraRotator in the inspector.
 Suggested to call function during loading screen.*/

public class GrowthTester : MonoBehaviour
{
#if DEBUG
    public GameObject objectToEnable;
    public GrowthController growthController;
    public CinematicCameraRotator cameraRotator;

    void Start()
    {
        if (objectToEnable == null || growthController == null || cameraRotator == null)
        {
            Debug.LogError("Object to enable, GrowthController, or CinematicCameraRotator is not assigned!");
        }
    }

    void Update()
    {
        //x + m
        if (PlayerInputHandler.instance.DebugDeleteSaveInput.WasPerformedThisFrame() && PlayerInputHandler.instance.DebugDeleteSaveInput.WasPressedThisFrame())
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            if (growthController != null)
            {
                growthController.TriggerGrowth();
            }

            if (cameraRotator != null)
            {
                //cameraRotator.StartRotation();
            }
        }
    }
#endif
}
