using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TimeSettings timeSettings;


    [SerializeField] Light sun;
    [SerializeField] Light moon;
    [SerializeField] AnimationCurve lightIntensityCurve;
    [SerializeField] float maxSunIntensity = 1;
    [SerializeField] float maxMoonIntensity = 0.5f;

    [SerializeField] Color dayAmbientLight;
    [SerializeField] Color nightAmbientLight;
    [SerializeField] Volume volume;
    [SerializeField] Material skyboxMaterial;

    ColorAdjustments colorAdjustments;


    [SerializeField] TimeSettings timeSettingss;



    TimeService service;

    [Header("Starry Sky Settings")]
    [SerializeField] private Material starMaterial;

    void Start()
    {
        service = new TimeService(timeSettingss);
        volume.profile.TryGet(out colorAdjustments);

        service.currentHour.ValueChanged += ToggleStarVisibility;
    }

    private void OnDisable()
    {
        service.currentHour.ValueChanged -= ToggleStarVisibility;
    }

    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateSkyBlend();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    timeSettingss.timeMultiplier *= 2;
        //}
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    timeSettingss.timeMultiplier /= 2;
        //}
    }

    void UpdateSkyBlend()
    {
        float dotProduct = Vector3.Dot(lhs: sun.transform.forward, rhs: Vector3.up);
        float blend = Mathf.Lerp(a: 0, b: 1, t: lightIntensityCurve.Evaluate(dotProduct));
        skyboxMaterial.SetFloat("_Blend", blend);
    }

    void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(lhs: sun.transform.forward, rhs: Vector3.down);
        sun.intensity = Mathf.Lerp(a:0, b: maxSunIntensity, t: lightIntensityCurve.Evaluate(dotProduct));
        moon.intensity = Mathf.Lerp(a: maxMoonIntensity, b: 0, t: lightIntensityCurve.Evaluate(dotProduct));
        if (colorAdjustments == null) return;
        colorAdjustments.colorFilter.value = Color.Lerp(a: nightAmbientLight, b: dayAmbientLight, t: lightIntensityCurve.Evaluate(dotProduct));
    }

    void RotateSun()
    {
        float rotation = service.CalculateSunAngle();
        sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
    }

    void UpdateTimeOfDay()
    {
        service.UpdateTime(Time.deltaTime);
        if (timeText != null)
        {
            timeText.text = service.CurrentTime.ToString(format: "hh:mm");
        }
    }

    private void ToggleStarVisibility(int hour)
    {
        if(hour == 18)
        {
            Debug.Log("About to show stars");
            StartCoroutine(ChangeStarAlpha(false));
        }

        else if(hour == 6)
        {
            Debug.Log("About to hide stars");
            StartCoroutine(ChangeStarAlpha(true));
        }
    }

    private IEnumerator ChangeStarAlpha(bool changeAlphaToZero)
    {
        Color currentColor = starMaterial.color;

        if (changeAlphaToZero)
        {
            float currentValue = 1f;
            float targetValue = 0f;

            while (currentValue != targetValue)
            {
                currentValue -= 0.01f;
                starMaterial.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentValue);

                yield return null;
            }
        }

        else
        {
            float currentValue = 0f;
            float targetValue = 1f;

            while (currentValue != targetValue)
            {
                currentValue += 0.01f;
                starMaterial.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentValue);

                yield return null;
            }
        }
    }
}