using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weather
{
    Clear,
    Cloudy,
    Rainy,
    HeavyRain,
    Foggy,
    Snow,
    Blizzard,
    Stormy,
    Rainbow
}

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private float startTime; // 하루를 1로 계산
    [SerializeField] private float timeSpeed; // 시간 흐름 배율
    [HideInInspector] public bool timeStopped;
    private float gameTime;                   // 현재 시간
    private float deltaTimeRatio;

    [HideInInspector] public bool isDayTime {  get; private set; }

    [HideInInspector] public Weather weather {  get; private set; }

    private void Start()
    {
        gameTime = startTime;
        deltaTimeRatio = timeSpeed / 86400f;

        weather = Weather.Clear;
    }

    void Update()
    {
        UpdateGameTime();
        UpdateWeather();
        UpdateDayNightCycle();
    }

    void UpdateGameTime()
    {
        if (timeStopped) return;

        gameTime += Time.deltaTime * deltaTimeRatio;
    }

    void UpdateDayNightCycle()
    {
        // 낮과 밤 전환 로직
    }

    void UpdateWeather()
    {
        // 날씨 갱신 로직
    }
}
