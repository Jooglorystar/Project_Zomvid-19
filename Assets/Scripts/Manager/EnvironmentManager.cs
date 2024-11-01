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
    [SerializeField] private float startTime; // �Ϸ縦 1�� ���
    [SerializeField] private float timeSpeed; // �ð� �帧 ����
    [HideInInspector] public bool timeStopped;
    private float gameTime;                   // ���� �ð�
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
        // ���� �� ��ȯ ����
    }

    void UpdateWeather()
    {
        // ���� ���� ����
    }
}
