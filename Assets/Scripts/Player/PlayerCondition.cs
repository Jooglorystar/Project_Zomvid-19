using System;
using TMPro;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public StatusUI uiCondition;
    public float damageWhenHungry = 15;
    public float damageWhenCold = 15;

    private Condition HP { get { return uiCondition.HP; } }
    private Condition Stamina { get { return uiCondition.Stamina; } }
    private Condition Hunger { get { return uiCondition.Hunger; } }
    private Condition Thirst { get { return uiCondition.Thirst; } }
    private Condition Temperature { get { return uiCondition.Temperature; } }

    public event Action OnTakeDamage;
    public float PlayerTemperature { get; private set; }

    [Header("Temperature")]
    public float dayTemperature = 30;
    public float nightTemperature = 20;
    public float cloudyDiff = 5;
    public float rainyDiff = 10;
    public float snowyDiff = 20;
    public bool nearFire;

    public TextMeshProUGUI curTemperature;

    private void Update()
    {
        if(Hunger.curValue <= 0 || Thirst.curValue <= 0)
        {
            HP.Subtract(damageWhenHungry * Time.deltaTime);
        }
        HP.Add(HP.PassiveValue * Time.deltaTime);
        Hunger.Subtract(Hunger.PassiveValue * Time.deltaTime);
        Thirst.Subtract(Thirst.PassiveValue * Time.deltaTime);
        Stamina.Add(Stamina.PassiveValue * Time.deltaTime);

        SetPlayerTemperature();
        HandleTemperatureDebuff();
        curTemperature.text = PlayerTemperature.ToString() + "\n" + Temperature.curValue + "\n" + CharacterManager.Instance.player.controller.rb.velocity.magnitude;
    }

    public void HandleTemperatureDebuff()
    {
        if(Temperature.curValue < 5 || Temperature.curValue > 35)
        {
            HP.Subtract(damageWhenCold * Time.deltaTime);
            CharacterManager.Instance.player.controller.debuff = 0.5f;
        }
        else if (Temperature.curValue < 20)
        {
            CharacterManager.Instance.player.controller.debuff = 0.75f;
        }
        else
        {
            CharacterManager.Instance.player.controller.debuff = 1f;
        }
    }

    private void SetPlayerTemperature()
    {
        float TemperDiff = 0;
        if (EnvironmentManager.Instance.IsDayTime || nearFire)
        {
            PlayerTemperature = dayTemperature;
        }
        else
        {
            PlayerTemperature = nightTemperature;
        }

        switch (EnvironmentManager.Instance.CurrentWeather)
        {
            case Weather.Cloudy:
                TemperDiff += cloudyDiff;
                break;
            case Weather.Snowy:
                TemperDiff += snowyDiff;
                break;
            case Weather.Rainy:
                TemperDiff += rainyDiff;
                break;
        }
        PlayerTemperature -= TemperDiff;
        SetTemperature(PlayerTemperature);
    }

    public void TakeDamage(float damage)
    {
        HP.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public void Heal(float amount)
    {
        HP.Add(amount);
    }

    public void GetStamina(float amount)
    {
        Stamina.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if(Stamina.curValue < amount)
        {
            return false;
        }
        else
        {
            Stamina.Subtract(amount);
            return true;
        }
    }

    public void Eat(float amount)
    {
        Hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        Thirst.Add(amount);
    }

    public void SetTemperature(float targetTemperature)
    {
        Temperature.StartControlValueCoroutine(targetTemperature);
    }

    public void GetWarm(float amount)
    {
        Temperature.Add(amount);
    }

    public void BeCold(float amount)
    {
        Temperature.Subtract(amount);
    }
}