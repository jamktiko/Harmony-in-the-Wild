using UnityEngine;

public class BioluminescentLight : MonoBehaviour
{
    public Light targetLight;  // Assign your spotlight in the Inspector
    public float minIntensity = 0f;  // Intensity at start and end of the cycle
    public float maxIntensity = 1f;  // Peak brightness at 50% of the cycle
    public float cycleDuration = 15f;  // Matches the VFX particle lifetime

    private float timer = 0f;

    void Update()
    {
        if (targetLight == null) return;

        // Calculate normalized time in the cycle (0 to 1)
        float normalizedTime = (timer % cycleDuration) / cycleDuration;

        // Shape the intensity curve to match the VFX lifetime gradient
        float intensityFactor = Mathf.Sin(normalizedTime * Mathf.PI);
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityFactor);

        // Update the timer
        timer += Time.deltaTime;
    }
}
