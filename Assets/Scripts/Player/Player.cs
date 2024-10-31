using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal PlayerData data;
    internal PlayerController controller;
    internal PlayerCondition condition;
    internal Equipment equip;

    private void Awake()
    {
        CharacterManager.Instance.player = this;
        equip = GetComponent<Equipment>();
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        data = GetComponent<PlayerData>();
    }
}
