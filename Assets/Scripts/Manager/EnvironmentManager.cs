using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


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
    [HideInInspector] public int WorldTimeDay { get; private set; }       // ���� ��¥
    [HideInInspector] public float WorldTimeHour { get; private set; }    // ���� �ð�(�Ҽ�����)
    private readonly PriorityQueue<Action> alarm = new();
    private float deltaTimeRatio;

    [Header("Environment Light")]
    [SerializeField] private Vector3 noon; // vector 90 0 0
    [SerializeField] private Light Sun;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;
    [SerializeField] private AnimationCurve lightingIntensityMultiplier;
    [SerializeField] private AnimationCurve reflectionIntensityMultiplier;

    [Header("Day and Weather")]
    [SerializeField] private WeatherSO dayWeatherSO;
    [SerializeField] private WeatherSO nightWeatherSO;
    [SerializeField] private WeatherSO cloudDayWeatherSO;
    [SerializeField] private WeatherSO cloudNightWeatherSO;
    [SerializeField] private float weatherTransitionTime;    // ���� : �Ϸ�
    [SerializeField] private Material blendedSkyboxMaterial; // ��ī�̹ڽ� Ʈ�������� ���� Ư�� ���۵� ���̴��� ����ϴ� Material
    [SerializeField] private float weatherChangeIntervalMin;
    [SerializeField] private float weatherChangeIntervalMax;
    [SerializeField] private float weatherClearWeight;  // ���� ����ġ
    [SerializeField] private float weatherCloudyWeight; // ���� ����ġ
    [SerializeField] private float weatherRainyWeight;  // ���� ����ġ
    [SerializeField] private float weatherSnowyWeight;  // ���� ����ġ
    private float sumWeatherWeight1, sumWeatherWeight2, sumWeatherWeight3, sumWeatherWeight4;

    [HideInInspector] public bool IsDayTime { get; private set; }         // ������ ����
    [HideInInspector] public Weather CurrentWeather { get; private set; } // ���� ����
    private bool isTargetNight;
    private Weather reservedWeather;
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
        RenderSettings.skybox = LerpWeatherOption(currentWeatherOption, currentWeatherOption, currentWeatherOption, 0.5f, 0).SkyBox;
        RenderSettings.fogColor = currentWeatherOption.FogColor;

        sumWeatherWeight1 = weatherClearWeight;
        sumWeatherWeight2 = weatherClearWeight + weatherCloudyWeight;
        sumWeatherWeight3 = weatherClearWeight + weatherCloudyWeight + weatherRainyWeight;
        sumWeatherWeight4 = weatherClearWeight + weatherCloudyWeight + weatherRainyWeight + weatherSnowyWeight;
        ReserveNextWeather();
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
        WorldTimeDay = (int)Mathf.Ceil(WorldTime);
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
        if (WorldTimeHour < 0.2 || WorldTimeHour > 0.8)
        {
            IsDayTime = false;
        }
        else
        {
            IsDayTime = true;
        }

        UpdateLighting(Sun, sunColor, sunIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(WorldTimeHour);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(WorldTimeHour);
    }

    private void UpdateWeather()
    {
        DayNightTransition();
        WeatherTransition();
        ApplyWeatherTransition();
    }

    private void DayNightTransition()
    {
        if (dayNightTransition == false)
        {
            if (isTargetNight == false && WorldTimeHour > 0.8 - weatherTransitionTime / 2)
            {
                Debug.Log("����Ǳ� ����");
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
                Debug.Log($"��ħ�Ǳ� ����");
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
                Debug.Log("���� ��ȯ ��");
                dayNightTransition = false;

                currentWeatherOption = targetWeatherOption1;
                targetWeatherOption1 = targetWeatherOption2;

                startTransitionTime1 = startTransitionTime2;
            }
        }
    }
    private void WeatherTransition()
    {
        if (WorldTime >= targetWeatherTime)
        {
            CurrentWeather = targetWeather;
        }

        if (weatherTransition == false)
        {
            if (WorldTime > targetWeatherTime - weatherTransitionTime / 2)
            {
                Debug.Log("���� ���� ����");
                weatherTransition = true;
                targetWeather = reservedWeather;
                startWeatherTransitionTime = WorldTime;

                if (dayNightTransition == false)
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
            if (WorldTime > targetWeatherTime + weatherTransitionTime / 2)
            {
                Debug.Log("���� ��ȯ ��");
                weatherTransition = false;
                targetWeatherTime = float.MaxValue;

                currentWeatherOption = targetWeatherOption1;
                targetWeatherOption1 = targetWeatherOption2;

                startTransitionTime1 = startTransitionTime2;

                ReserveNextWeather(); // ���� ���� ����
            }
        }
    }
    private void ApplyWeatherTransition()
    {
        WeatherOption lerpWeatherOption = null;
        if (dayNightTransition || weatherTransition)
        {
            if (!dayNightTransition || !weatherTransition)
            {
                lerpWeatherOption = LerpWeatherOption(currentWeatherOption, targetWeatherOption1, currentWeatherOption, (WorldTime - startTransitionTime1) / weatherTransitionTime, 0);
            }
            else
            {
                lerpWeatherOption = LerpWeatherOption(currentWeatherOption, targetWeatherOption1, targetWeatherOption2, (WorldTime - startTransitionTime1) / weatherTransitionTime, (WorldTime - startTransitionTime2) / weatherTransitionTime);
            }
        }

        if (lerpWeatherOption != null)
        {
            RenderSettings.skybox = lerpWeatherOption.SkyBox;
            RenderSettings.fogColor = lerpWeatherOption.FogColor;
        }
    }

    public void ReserveNextWeather()
    {
        float nextTransitionTime = WorldTime + Random.Range(weatherChangeIntervalMin, weatherChangeIntervalMax);
        float nextWeather = Random.Range(0, sumWeatherWeight4);

        Weather nextTransitionWeather = Weather.Clear;
        if (nextWeather < sumWeatherWeight1) nextTransitionWeather = Weather.Clear;
        else if (nextWeather < sumWeatherWeight2) nextTransitionWeather = Weather.Cloudy;
        else if (nextWeather < sumWeatherWeight3) nextTransitionWeather = Weather.Rainy;
        else nextTransitionWeather = Weather.Snowy;
        
        targetWeatherTime = nextTransitionTime;
        reservedWeather = nextTransitionWeather;
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

    private WeatherOption LerpWeatherOption(WeatherOption weather1, WeatherOption weather2, WeatherOption weather3, float t1, float t2)
    {
        WeatherOption weatherOption = new()
        {
            SkyBox = weather1.SkyBox
        };
        
        blendedSkyboxMaterial.SetTexture("_Tex1", weather1.SkyBox.mainTexture);
        blendedSkyboxMaterial.SetTexture("_Tex2", weather2.SkyBox.mainTexture);
        blendedSkyboxMaterial.SetTexture("_Tex3", weather3.SkyBox.mainTexture);
        blendedSkyboxMaterial.SetFloat("_Exposure1", weather1.SkyBox.GetFloat("_Exposure"));
        blendedSkyboxMaterial.SetFloat("_Exposure2", weather2.SkyBox.GetFloat("_Exposure"));
        blendedSkyboxMaterial.SetFloat("_Exposure3", weather3.SkyBox.GetFloat("_Exposure"));
        blendedSkyboxMaterial.SetFloat("_Blend", t1);
        blendedSkyboxMaterial.SetFloat("_Blend2", t2);
        weatherOption.SkyBox = blendedSkyboxMaterial;

        if (t2 == 0)
        {
            weatherOption.FogColor = Color.Lerp(weather1.FogColor, weather2.FogColor, t1);
        }
        else
        {
            weatherOption.FogColor = Color.Lerp(Color.Lerp(weather1.FogColor, weather2.FogColor, t1), weather3.FogColor, t2);
        }

        return weatherOption;
    }



    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(WorldTimeHour);

        if (0.25 < WorldTimeHour && WorldTimeHour < 0.75)
        {
            lightSource.transform.eulerAngles = (WorldTimeHour - 0.25f) * noon * 4f;
        }
        else
        {
            lightSource.transform.eulerAngles = (WorldTimeHour - 0.75f) * noon * 4f;
        }
        lightSource.color = gradient.Evaluate(WorldTimeHour);
        lightSource.intensity = intensity;
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
