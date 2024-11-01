﻿using TMPro;
using UnityEngine;

public class ItemGainValue
{
    public ItemSO itemData;
    public int itemCount;
}

public class UIInventoryTab : MonoBehaviour
{
    public ItemSlot[] slots;

    public Transform Page1;
    public Transform Page2;

    [Header("Select Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;

    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDesc;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject BuildButton;
    public GameObject dropButton;

    private void Start()
    {
        slots = new ItemSlot[Page1.childCount + Page2.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Page1.childCount)
            {
                slots[i] = Page1.GetChild(i).GetComponent<ItemSlot>();
            }
            else if (i >= Page1.childCount)
            {
                slots[i] = Page2.GetChild(i - Page1.childCount).GetComponent<ItemSlot>();
            }

            slots[i].index = i;
            slots[i].inventoryTab = this;
        }

        ClearInventorySelectedItemWindow();
    }

    private void ClearInventorySelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDesc.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        BuildButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void SelectItem(int index)
    {
        if (slots[index].itemData == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemData.itemName;
        selectedItemDesc.text = selectedItem.itemData.itemDesc;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; 0 < selectedItem.itemData.ConsumeData.Count; i++)
        {
            selectedItemStatName.text += selectedItem.itemData.ConsumeData[i].consumeType.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.itemData.ConsumeData[i].itemEffectValue.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.itemData.itemType == ItemType.Consume);
        BuildButton.SetActive(selectedItem.itemData.itemType == ItemType.Build);
    }

    private void AddItem()
    {
        // 중복 확인

        // 빈 슬롯 확인

        // 빈 슬롯 있음

        // 빈 슬롯 없음
    }
}