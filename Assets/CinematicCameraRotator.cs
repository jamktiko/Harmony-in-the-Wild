using UnityEngine;

// Rotates the cinematic camera around a fixed point over 10 seconds.
public class CinematicCameraRotator : MonoBehaviour
{
    public float duration = 10.0f;
    private Quaternion originalRotation;
    private bool rotating = false;
    private float elapsedTime = 0.0f;

    private void Awake()
    {
        StartRotation();
    }

    void Start()
    {
        originalRotation = transform.rotation;
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
            }
        }
    }

    public void StartRotation()
    {
        rotating = true;
    }

    public void StopRotation()
    {
        rotating = false;
        elapsedTime = 0.0f;
        transform.rotation = originalRotation;
    }
}