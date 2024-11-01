using System;
using Unity.VisualScripting;
using UnityEngine;


public enum Weather
{
    Clear,
    Cloudy,
    Rainy,
    Snowy
}

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    [Header("World Time Data")]
    [SerializeField] private float startTime;  // 하루를 1로 계산
    [SerializeField] private float timeSpeed;  // 시간 흐름 배율
    [HideInInspector] public bool TimeStopped;                            // 시간 멈춤
    [HideInInspector] public float WorldTime { get; private set; }        // 현재 시간
    [HideInInspector] public float WorldTimeHour { get; private set; }    // 현재 시간(소수점만)
    private readonly PriorityQueue<Action> alarm = new();
    private float deltaTimeRatio;

    [Header("Environment Light")]
    [SerializeField] private Vector3 noon; // vector 90 0 0
    [SerializeField] private Light Sun;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;
    [SerializeField] private Light Moon;
    [SerializeField] private Gradient moonColor;
    [SerializeField] private AnimationCurve moonIntensity;
    [SerializeField] private AnimationCurve lightingIntensityMultiplier;
    [SerializeField] private AnimationCurve reflectionIntensityMultiplier;

    [Header("Day and Weather")]
    [SerializeField] private WeatherSO dayWeatherSO;
    [SerializeField] private WeatherSO nightWeatherSO;
    [SerializeField] private WeatherSO cloudDayWeatherSO;
    [SerializeField] private WeatherSO cloudNightWeatherSO;
    [SerializeField] private float weatherTransitionTime; // 단위 : 하루
    [HideInInspector] public bool IsDayTime { get; private set; }         // 낮인지 여부
    [HideInInspector] public Weather CurrentWeather { get; private set; } // 현재 날씨
    private bool isTargetNight;
    private Weather targetWeather;
    private float targetWeatherTime;
    private bool dayNightTransition;
    private bool weatherTransition;
    private WeatherOption currentWeatherOption;
    private WeatherOption targetWeatherOption1;
    private WeatherOption targetWeatherOption2;
    private float startDayTransitionTime;
    private float startWeatherTransitionTime;
    private float startTransitionTime1;
    private float startTransitionTime2;

    private void Start()
    {
        WorldTime = startTime;
        deltaTimeRatio = timeSpeed / 86400f;

        CurrentWeather = Weather.Clear;
        targetWeather = Weather.Clear;

        currentWeatherOption = dayWeatherSO.weathers[0];
        RenderSettings.skybox = currentWeatherOption.SkyBox;
        RenderSettings.fogColor = currentWeatherOption.FogColor;
    }

    private void Update()
    {
        UpdateWorldTime();
        UpdateDayNightCycle();
        UpdateWeather();
    }

    private void UpdateWorldTime()
    {
        if (TimeStopped) return;

        WorldTime += Time.deltaTime * deltaTimeRatio;
        WorldTimeHour = WorldTime % 1.0f;

        // 가장 첫 알람 시간과 비교해서 시간이 지났으면 시간이 지난 모든 알람 발생
        if (WorldTime >= alarm.PeekPriority())
        {
            do
            {
                if (alarm.Dequeue(out Action callback))
                {
                    callback?.Invoke();
                }
            }
            while (WorldTime >= alarm.PeekPriority());
        }
    }

    private void UpdateDayNightCycle()
    {
        if (WorldTimeHour < 0.2 || WorldTimeHour > 0.8)
        {
            IsDayTime = false;
        }
        else
        {
            IsDayTime = true;
        }

        UpdateLighting(Sun, sunColor, sunIntensity);
        UpdateLighting(Moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(WorldTimeHour);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(WorldTimeHour);
    }

    private void UpdateWeather()
    {
        if (WorldTime >= targetWeatherTime)
        {
            CurrentWeather = targetWeather;
        }

        if (dayNightTransition == false)
        {
            if (isTargetNight == false && WorldTimeHour > 0.8 - weatherTransitionTime / 2)
            {
                Debug.Log("저녁되기 시작");
                dayNightTransition = true;
                isTargetNight = true;
                startDayTransitionTime = WorldTime;

                if (weatherTransition == false)
                {
                    targetWeatherOption1 = GetWeatherOption(isTargetNight, targetWeather);
                    startTransitionTime1 = WorldTime;
                }
                else
                {
                    targetWeatherOption2 = GetWeatherOption(isTargetNight, targetWeather);
                    startTransitionTime2 = WorldTime;
                }
            }
            else if (isTargetNight == true && WorldTimeHour > 0.2 - weatherTransitionTime / 2 && WorldTimeHour < 0.5)
            {
                Debug.Log($"아침되기 시작");
                dayNightTransition = true;
                isTargetNight = false;
                startDayTransitionTime = WorldTime;

                if (weatherTransition == false)
                {
                    targetWeatherOption1 = GetWeatherOption(isTargetNight, targetWeather);
                    startTransitionTime1 = WorldTime;
                }
                else
                {
                    targetWeatherOption2 = GetWeatherOption(isTargetNight, targetWeather);
                    startTransitionTime2 = WorldTime;
                }
            }
        }
        else
        {
            if (WorldTime > startDayTransitionTime + weatherTransitionTime)
            {
                Debug.Log("전환 끝");
                dayNightTransition = false;

                currentWeatherOption = targetWeatherOption1;
                targetWeatherOption1 = targetWeatherOption2;

                startTransitionTime1 = startTransitionTime2;
            }
        }

        WeatherOption nextWeatherOption = null;
        if (dayNightTransition || weatherTransition)
        {
            nextWeatherOption = LerpWeatherOption(currentWeatherOption, targetWeatherOption1, (WorldTime - startTransitionTime1) / weatherTransitionTime);

            if (dayNightTransition && weatherTransition)
            {
                nextWeatherOption = LerpWeatherOption(nextWeatherOption, targetWeatherOption2, (WorldTime - startTransitionTime2) / weatherTransitionTime);
            }
        }

        if (nextWeatherOption != null)
        {
            RenderSettings.skybox = nextWeatherOption.SkyBox;
            RenderSettings.fogColor = nextWeatherOption.FogColor;
        }
    }
    
    private WeatherOption GetWeatherOption(bool isNight, Weather weather)
    {
        WeatherOption weatherOption = new WeatherOption();
        if (isNight == false)
        {
            switch (weather)
            {
                case Weather.Clear:
                    weatherOption = dayWeatherSO.weathers[UnityEngine.Random.Range(0, dayWeatherSO.weathers.Count)];
                    break;
                case Weather.Cloudy:
                case Weather.Rainy:
                case Weather.Snowy:
                    weatherOption = cloudDayWeatherSO.weathers[UnityEngine.Random.Range(0, cloudDayWeatherSO.weathers.Count)];
                    break;
            }
        }
        else
        {
            switch (weather)
            {
                case Weather.Clear:
                    weatherOption = nightWeatherSO.weathers[UnityEngine.Random.Range(0, nightWeatherSO.weathers.Count)];
                    break;
                case Weather.Cloudy:
                case Weather.Rainy:
                case Weather.Snowy:
                    weatherOption = cloudNightWeatherSO.weathers[UnityEngine.Random.Range(0, cloudNightWeatherSO.weathers.Count)];
                    break;
            }
        }

        return weatherOption;
    }

    private WeatherOption LerpWeatherOption(WeatherOption weather1, WeatherOption weather2, float t)
    {
        WeatherOption weatherOption = new WeatherOption();
        weatherOption.SkyBox = weather1.SkyBox;

        weatherOption.SkyBox.Lerp(weather1.SkyBox, weather2.SkyBox, t);
        weatherOption.FogColor = Color.Lerp(weather1.FogColor, weather2.FogColor, t);

        return weatherOption;
    }



    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(WorldTimeHour);

        lightSource.transform.eulerAngles = (WorldTimeHour - (lightSource == Sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(WorldTimeHour);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }



    public void SetAlarm(float targetTime, Action callback)
    {
        alarm.Enqueue(callback, targetTime);
    }
    public void SetAlarmToday(float targetTimeHour, Action callback)
    {
        float targetTime = Mathf.Floor(WorldTime) + (targetTimeHour % 1.0f);
        alarm.Enqueue(callback, targetTime);
    }
}
