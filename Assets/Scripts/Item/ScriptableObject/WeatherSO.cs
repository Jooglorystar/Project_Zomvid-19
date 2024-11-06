using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeatherOption
{
    public Material SkyBox;
    public Color FogColor;
}

[CreateAssetMenu(fileName = "WeatherSO", menuName = "WeatherOptionSO", order = 3)]
public class WeatherSO : ScriptableObject
{
    public List<WeatherOption> weathers;
}
