using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    internal PlayerData playerData;
    internal PlayerController controller;
    internal PlayerCondition condition;
    public Equipment equip;
    internal ItemSO itemData;

    internal UIInventoryTab uiInventoryTab;
    internal UICraftingTab uICraftingTab;

    internal Action AddItem;
    internal Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.player = this;
        equip = GetComponent<Equipment>();
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        playerData = GetComponent<PlayerData>();
    }
}
