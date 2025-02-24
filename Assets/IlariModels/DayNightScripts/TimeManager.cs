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

    [Header("Northern Lights")]
    [SerializeField] private GameObject auroraHolder;

    [Header("Fog Color")]
    [SerializeField] private float fogTransitionDuration = 2f;
    [SerializeField] private Color dayFog;
    [SerializeField] private Color nightFog;

    void Start()
    {
        service = new TimeService(timeSettingss);
        volume.profile.TryGet(out colorAdjustments);

        InitializeStarColor();

        service.currentHour.ValueChanged += ToggleDayNightCycleElements;
    }

    private void OnDisable()
    {
        service.currentHour.ValueChanged -= ToggleDayNightCycleElements;
    }

    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateSkyBlend();

        if (PlayerInputHandler.instance.DebugToggleDayNight.WasPressedThisFrame())
        {
            timeSettingss.timeMultiplier *= 100f;
            Invoke(nameof(ResetTimeMultiplier), 4f);
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    timeSettingss.timeMultiplier *= 2;
        //}
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    timeSettingss.timeMultiplier /= 2;
        //}
    }

    private void ResetTimeMultiplier()
    {
        timeSettings.timeMultiplier = 100f;
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

    private void ToggleDayNightCycleElements(int hour)
    {
        if(hour == 18)
        {
            Debug.Log("About to show night elements");
            ToggleAuroraVisibility(true);
            StartCoroutine(ChangeFogColor(dayFog, nightFog));
            StartCoroutine(ChangeStarAlpha(false));
        }

        else if(hour == 6)
        {
            Debug.Log("About to hide night elements");
            ToggleAuroraVisibility(false);
            StartCoroutine(ChangeFogColor(nightFog, dayFog));
            StartCoroutine(ChangeStarAlpha(true));
        }
    }

    private void ToggleAuroraVisibility(bool auroraVisible)
    {
        auroraHolder.SetActive(auroraVisible);
    }

    private IEnumerator ChangeStarAlpha(bool changeAlphaToZero)
    {
        Color currentColor = starMaterial.color;

        if (changeAlphaToZero)
        {
            float currentValue = 1f;
            float targetValue = 0f;

            while (currentValue > targetValue)
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

            while (currentValue < targetValue)
            {
                currentValue += 0.01f;
                starMaterial.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentValue);

                yield return null;
            }
        }
    }

    private void InitializeStarColor()
    {
        if (service.IsDayTime())
        {
            starMaterial.color = new Color(starMaterial.color.r, starMaterial.color.g, starMaterial.color.b, 0);
        }

        else
        {
            starMaterial.color = new Color(starMaterial.color.r, starMaterial.color.g, starMaterial.color.b, 1);
        }
    }

    private IEnumerator ChangeFogColor(Color currentColor, Color targetColor)
    {
        float elapsedTime = 0f;

        while(elapsedTime < fogTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            Color newColor = Color.Lerp(currentColor, targetColor, elapsedTime / fogTransitionDuration);
            RenderSettings.fogColor = newColor;
            yield return null;
        }
    }
}