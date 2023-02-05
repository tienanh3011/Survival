using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Transform sun;
    [Header("Cycle Setting")]
    public float timeOfDay = 1350;
    public float cycleDuration = 2700f;
    public float dayStartTime = 750f;
    public float dayEndTime = 2150f;
    [Space]
    public float cycleSpeed = 1f;



    [Header("Lighting Setting")]
    public float dayTimeSunIntensity = 1f;
    public float nightTimeSunIntensity = 0;
    [Space]
    public float dayTimeAmbientIntensity = 1;
    public float nightTimeAmbientIntensity = 0.15f;
    [Space]
    [Space]
    public float intensityChangeSpeed = 1f;
    public Material skybox;
    public Color dayTimeColor;
    public Color nightTimeColor;

    [HideInInspector] public bool isNightTime;

    private void Start()
    {
        if (!isNightTime)
        {
            sun.GetComponentInChildren<Light>().intensity = dayTimeSunIntensity;
        }
        else
        {
            sun.GetComponentInChildren<Light>().intensity = nightTimeSunIntensity;
        }
    }
    private void Update()
    {
        if (!isNightTime)
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, dayTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, dayTimeAmbientIntensity, intensityChangeSpeed * Time.deltaTime);
            if (skybox != null) ;
            RenderSettings.skybox.SetColor("Color_",Color.Lerp(skybox.color, dayTimeColor, intensityChangeSpeed * Time.deltaTime));
        }
        else
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, nightTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, nightTimeAmbientIntensity, intensityChangeSpeed * Time.deltaTime);
            if (skybox != null) ;
            RenderSettings.skybox.SetColor("Color_", Color.Lerp(skybox.color, nightTimeColor, intensityChangeSpeed * Time.deltaTime));

        }
        if (timeOfDay > cycleDuration)
        {
            timeOfDay = 0;
        }
        if (timeOfDay > dayStartTime && timeOfDay < dayEndTime)
        {
            timeOfDay += cycleSpeed * Time.deltaTime;
        }
        else
        {
            timeOfDay += (cycleSpeed * 2) * Time.deltaTime;
        }
        UpdateLighting();
    }
    public void UpdateLighting()
    {
        sun.localRotation = Quaternion.Euler((timeOfDay * 360 / cycleDuration), 0, 0);

        if (timeOfDay < dayStartTime|| timeOfDay>dayEndTime)
        {
            isNightTime = true;

        }
        else
        {
            isNightTime = false;
        }
    }
}
