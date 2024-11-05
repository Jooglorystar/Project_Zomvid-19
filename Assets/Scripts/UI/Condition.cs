using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float PassiveValue;
    public float startValue;
    public float maxValue;
    public float curValue;
    private Image ConditionUI;

    private void Awake()
    {
        ConditionUI = GetComponent<Image>();
    }

    private void Update()
    {
        ConditionUI.fillAmount = GetPercentage();
    }

    private void Start()
    {
        curValue = startValue;
    }


    public float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float amount)
    {
        curValue = Mathf.Clamp(curValue += amount, 0, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Clamp(curValue -= amount, 0, maxValue);
    }

    public void MaxValueChange(float amount)
    {
        maxValue = Mathf.Max(0, maxValue + amount);
    }

    public IEnumerator ControlValue(float targetValue)
    {
        float increment = 0.1f;
        increment = targetValue < curValue? -increment : increment;

        while (Mathf.Approximately(curValue, targetValue))
        {
            yield return null;
            yield return null;
            curValue += increment;
        }
    }
}
