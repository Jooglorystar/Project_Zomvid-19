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
    [SerializeField] private float startTime;  // �Ϸ縦 1�� ���
    [SerializeField] private float timeSpeed;  // �ð� �帧 ����
    [HideInInspector] public bool TimeStopped;                            // �ð� ����
    [HideInInspector] public float WorldTime { get; private set; }        // ���� �ð�
    [HideInInspector] public float WorldTimeHour { get; private set; }    // ���� �ð�(�Ҽ�����)
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
    [SerializeField] private float transitionTime; // ���� : �Ϸ�
    [HideInInspector] public bool IsDayTime { get; private set; }         // ������ ����
    [HideInInspector] public Weather CurrentWeather { get; private set; } // ���� ����

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

        // ���� ù �˶� �ð��� ���ؼ� �ð��� �������� �ð��� ���� ��� �˶� �߻�
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
        // ���� ���� ����
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
