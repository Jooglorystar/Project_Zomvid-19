﻿using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private Image slotIcon;

    public void Start()
    {
        // 임시
        if (itemData != null) Set();
        else Clear();
    }
    public void Set()
    {
        slotIcon.gameObject.SetActive(true);
        slotIcon.sprite = itemData.icon;
    }

    public void Clear()
    {
        itemData = null;
        slotIcon.gameObject.SetActive(false);
    }
}