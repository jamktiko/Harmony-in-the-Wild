using UnityEngine;

public class BioluminescentLight : MonoBehaviour
{
    public Light targetLight; // Assign your spotlight here in the Inspector
    public float minIntensity = 0.5f; // Minimum light intensity
    public float maxIntensity = 5.0f; // Maximum light intensity at peak
    public float cycleDuration = 15f; // Matches the particle lifetime
    private float timer = 0f;

    void Update()
    {
        if (targetLight == null) return;

        // Calculate normalized time in the cycle (0 to 1)
        float normalizedTime = (timer % cycleDuration) / cycleDuration;

        // Create a "breathing" effect using a sine wave
        float intensityFactor = Mathf.Sin(normalizedTime * Mathf.PI); // Peaks at 0.5, fades at 0 & 1

        // Scale intensity based on min/max range
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityFactor);

        // Update the timer
        timer += Time.deltaTime;
    }
}
