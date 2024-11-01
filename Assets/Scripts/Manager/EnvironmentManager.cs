using System;
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
    [SerializeField] private Material[] day;
    [SerializeField] private Material[] night;
    [SerializeField] private Material[] cloudDay;
    [SerializeField] private Material[] cloudNight;
    [SerializeField] private float transitionTime; // 단위 : 하루
    [HideInInspector] public bool IsDayTime { get; private set; }         // 낮인지 여부
    [HideInInspector] public Weather CurrentWeather { get; private set; } // 현재 날씨

    private void Start()
    {
        WorldTime = startTime;
        deltaTimeRatio = timeSpeed / 86400f;

        CurrentWeather = Weather.Clear;
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
        if (WorldTimeHour < 0.25 || WorldTimeHour > 0.8)
        {
            IsDayTime = false;
            //RenderSettings.skybox = night;
            
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
        // 날씨 갱신 로직
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
