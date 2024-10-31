using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    public Condition HP;
    public Condition Stamina;
    public Condition Hunger;
    public Condition Thirst;
    public Condition Temperature;

    private void Start()
    {
        CharacterManager.Instance.player.condition.uiCondition = this;

        HP.maxValue = CharacterManager.Instance.player.data.maxHealth;
        Stamina.maxValue = CharacterManager.Instance.player.data.maxStamina;
        Hunger.maxValue = CharacterManager.Instance.player.data.maxHunger;
        Thirst.maxValue = CharacterManager.Instance.player.data.maxThirst;
        Temperature.maxValue = 30;
    }
}
