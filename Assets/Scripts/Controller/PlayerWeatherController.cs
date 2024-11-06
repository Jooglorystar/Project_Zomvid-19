using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeatherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private ParticleSystem snow;

    private void Update()
    {
        WeatherChange();
    }

    private void WeatherChange()
    {
        switch (EnvironmentManager.Instance.CurrentWeather)
        {
            case Weather.Rainy:
                Rain();
                break;
            case Weather.Snowy:
                Snow();
                break;
            case Weather.Cloudy:
            case Weather.Clear:
                break;
        }
    }

    public void Rain()
    {
        rain.Play();
        snow.Stop();
    }

    public void Snow()
    {
        snow.Play();
        rain.Stop();
    }
}
