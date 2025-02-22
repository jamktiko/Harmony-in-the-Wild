using UnityEngine;

public class BioluminescentLight : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1f; 
    public float cycleDuration = 15f;

    private float timer = 0f;

    void Update()
    {
        PulseLight();
    }

    private void PulseLight()
    {
        if (targetLight == null) return;

        float normalizedTime = (timer % cycleDuration) / cycleDuration;

        float intensityFactor = Mathf.Sin(normalizedTime * Mathf.PI);
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityFactor);

        timer += Time.deltaTime;
    }
}
