using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal PlayerData data;
    internal PlayerController controller;
    internal PlayerCondition condition;

    private void Awake()
    {
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        data = GetComponent<PlayerData>();
    }
}
