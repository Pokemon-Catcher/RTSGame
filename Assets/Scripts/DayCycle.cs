using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Header("Timer")]

    [SerializeField]
    [Range(0f, 1f)]
    private float timeOfDay;

    [SerializeField]
    private int dayCount = 0;
    public int DayCont
    {
        get { return dayCount; }
    }

    [SerializeField]
    private float dayLength = 0f;

    [SerializeField]
    private int yearCount = 0;
    public int YearCount
    {
        get { return yearCount; }
    }

    [SerializeField]
    private float yearLength = 0f;

    [SerializeField]
    private bool pause = false;

    [Header("Sun Props")]
    [SerializeField]
    private Transform sunTransform;

    [SerializeField]
    private Light sunlight;

    [SerializeField]
    private float baseIntensity;

    [SerializeField]
    private Gradient gradient;

    [Header("Day cycle")]
    [SerializeField]
    private bool isDay;

    private float intensity;

    private float timeScale;

    [System.Serializable]
    private struct RTSTime
    {
        public int hours;
        public int minutes;
        public int seconds;

        public RTSTime(int hours, int minutes, int seconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }
    }

    [SerializeField]
    private RTSTime time = new RTSTime(0,0,0);

    [SerializeField]
    private int fullSeconds;

    private void UpdateTime()
    {
        timeOfDay += (Time.deltaTime * timeScale) / (24 * 60 * 60);

        if(timeOfDay > 1)
        {
            dayCount++;
            timeOfDay = 0;

            if(dayCount > yearLength)
            {
                yearCount++;
                dayCount = 0;
            }
        }
    }

    private void UpdateTimeScale()
    {
        timeScale = 24 / (dayLength / 60);
    }

    private void UpdateSunRotationAndIntensityAndColor()
    {
        float angle = timeOfDay * 360f;

        sunTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        intensity = Vector3.Dot(sunTransform.up, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sunlight.intensity = intensity * 0.5f + baseIntensity;

        sunlight.color = gradient.Evaluate(intensity);
    }

    void CheckIsDay()
    {
        if (timeOfDay > 0.25f && timeOfDay < 0.75f)
            isDay = true;
        else
            isDay = false;
    }

    void UpdateDayTime()
    {
        fullSeconds = (int)(60 * 60 * 24 * timeOfDay);

        time.hours = fullSeconds / 3600;
        time.minutes = (fullSeconds - time.hours * 3600) / 60;
        time.seconds = fullSeconds - time.hours * 3600 - time.minutes * 60;
    }

    void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
            UpdateSunRotationAndIntensityAndColor();
            CheckIsDay();
            UpdateDayTime();
        }
    }
}
