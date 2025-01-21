using System;
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


    void Start()
    {
        service = new TimeService(timeSettingss);
        volume.profile.TryGet(out colorAdjustments);
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

}