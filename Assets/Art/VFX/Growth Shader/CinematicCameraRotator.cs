using UnityEngine;

// Rotates the cinematic camera around a fixed point over 10 seconds.
public class CinematicCameraRotator : MonoBehaviour
{
    public float duration = 10.0f;
    private Quaternion originalRotation;
    private bool rotating = false;
    private float elapsedTime = 0.0f;
    public GameObject objectToEnable;
    public GrowthController growthController;

    void Start()
    {
        originalRotation = transform.rotation;
        StartRotation();
    }

    void Update()
    {
        if (rotating)
        {
            elapsedTime += Time.deltaTime;
            float angle = Mathf.Lerp(0, 360, elapsedTime / duration);
            transform.rotation = originalRotation * Quaternion.Euler(0, angle, 0);

            if (elapsedTime >= duration)
            {
                transform.rotation = originalRotation;
                rotating = false;
                elapsedTime = 0.0f;
                if (objectToEnable != null)
                {
                    objectToEnable.SetActive(false);
                    if (growthController != null)
                    {
                        growthController.SaveGrowthValues();
                    }
                }

                GameEventsManager.instance.CinematicsEvents.EndCinematics();
            }
        }
    }

    public void StartRotation()
    {
        rotating = true;
        elapsedTime = 0.0f;
    }
}
