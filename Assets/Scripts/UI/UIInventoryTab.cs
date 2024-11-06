using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

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
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDesc;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    private ItemSlot selectedItem;
    private int selectedItemIndex;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject BuildButton;
    public GameObject dropButton;

    private Vector3 dropPos;
    private PlayerCondition condition;
    private PlayerController controller;

    private void Awake()
    {
    }

    private void Start()
    {
        CharacterManager.Instance.player.uiInventoryTab = this;
        condition = CharacterManager.Instance.player.condition;
        controller = CharacterManager.Instance.player.controller;

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

    private void OnDisable()
    {
        ClearInventorySelectedItemWindow();
    }

    // 창 초기화
    private void ClearInventorySelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDesc.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        ClearButtons();
    }

    private void ClearButtons()
    {
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        BuildButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 선택아이템 표시
    public void SelectItem(int index)
    {
        if (slots[index].itemData == null)
        {
            ClearInventorySelectedItemWindow();
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemData.itemName;
        selectedItemDesc.text = selectedItem.itemData.itemDesc;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.itemData.ConsumeData.Count; i++)
        {
            selectedItemStatName.text += selectedItem.itemData.ConsumeData[i].consumeType.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.itemData.ConsumeData[i].itemEffectValue.ToString() + "\n";
        }

        ClearButtons();

        switch (slots[index].itemData.itemType)
        {
            case ItemType.Consume:
                useButton.SetActive(true);
                break;
            case ItemType.Equip:
                if (slots[index].equipped)
                {
                    unequipButton.SetActive(true);
                    break;
                }
                else
                    equipButton.SetActive(true);
                break;
            case ItemType.Resource:
                break;
            case ItemType.Build:
                BuildButton.SetActive(true);
                break;
        }
        dropButton.SetActive(true);
    }

    public void OnDropButton()
    {
        // TODO 드랍 메서드
        if (slots[selectedItemIndex].equipped == true) return;
        ThrowItem(selectedItem.itemData);
        UpdateInventory();
        RemoveSelectedItem();
    }

    public void AddItem()
    {
        ItemSO data = CharacterManager.Instance.player.itemData;

        //아이템이 중복가능한지 canStack체크
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.itemCount++;
                UpdateInventory();
                CharacterManager.Instance.player.itemData = null;
                return;
            }
        }
        //비어있는 슬롯을 가져온다.
        ItemSlot emptySlot = GetEmptySlot();
        //비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.itemData = data;
            emptySlot.itemCount = 1;
            UpdateInventory();
            CharacterManager.Instance.player.itemData = null;
            return;
        }

        //없다면
        ThrowItem(data);
        CharacterManager.Instance.player.itemData = null;
    }

    public void AddItem(List<ItemStack> itemStacks)
    {
        foreach (ItemStack itemStack in itemStacks)
        {
            //아이템이 중복가능한지 canStack체크
            if (itemStack.itemSO.canStack)
            {
                while (itemStack.stack > 0)
                {
                    ItemSlot slot = GetItemStack(itemStack.itemSO);
                    if (slot != null)
                    {
                        if (slot.itemCount + itemStack.stack <= itemStack.itemSO.MaxStackSize)
                        {
                            slot.itemCount += itemStack.stack;
                            itemStack.stack = 0;
                        }
                        else
                        {
                            itemStack.stack += slot.itemCount - itemStack.itemSO.MaxStackSize;
                            slot.itemCount = itemStack.itemSO.MaxStackSize;
                        }
                    }
                    else
                    {
                        ItemSlot emptySlot = GetEmptySlot();
                        if (emptySlot != null)
                        {
                            emptySlot.itemData = itemStack.itemSO;
                            if (itemStack.stack <= itemStack.itemSO.MaxStackSize)
                            {
                                emptySlot.itemCount = itemStack.stack;
                                itemStack.stack = 0;
                            }
                            else
                            {
                                itemStack.stack -= itemStack.itemSO.MaxStackSize;
                                emptySlot.itemCount = itemStack.itemSO.MaxStackSize;
                            }
                        }
                        else
                        {
                            DropItem(itemStack);
                            itemStack.stack = 0;
                        }
                    }
                }
            }
            else
            {
                ItemSlot emptySlot = GetEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.itemData = itemStack.itemSO;
                    emptySlot.itemCount = 1;
                    UpdateInventory();
                    return;
                }
                else
                {
                    DropItem(itemStack);
                }
            }
        }
        CharacterManager.Instance.player.itemData = null;
        UpdateInventory();
    }

    private void DropItem(ItemStack itemStack)
    {
        dropPos = CharacterManager.Instance.player.transform.position + Vector3.up * 1.5f;
        GameObject drops = Instantiate(itemStack.itemSO.dropPrefab, dropPos, Quaternion.identity);
        drops.transform.parent = WorldLevelManager.Instance.ItemObjectsParent;
        var itemObject = drops.GetComponent<ItemObject>();
        if (itemObject != null)
        {
            itemObject.itemStack.stack = itemStack.stack;
        }
    }

    private void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    private void ThrowItem(ItemSO data)
    {
        dropPos = CharacterManager.Instance.player.transform.position + Vector3.up * 1.5f;
        Instantiate(data.dropPrefab, dropPos, Quaternion.identity);
    }

    private ItemSlot GetEmptySlot()
    {
        return slots.FirstOrDefault(slot => slot.itemData == null);
    }

    private ItemSlot GetItemStack(ItemSO data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == data && slots[i].itemCount < data.MaxStackSize)
            {
                return slots[i];
            }
        }
        return null;
    }

    public bool RemoveSelectedItem() //고갈 상태 반환
    {
        bool Depleted = false;

        slots[selectedItemIndex].itemCount--;
        if (slots[selectedItemIndex].itemCount <= 0)
        {
            slots[selectedItemIndex].itemCount = 0;
            slots[selectedItemIndex].itemData = null;

            Depleted = true;
        }
        UpdateInventory();

        return Depleted;
    }

    public void OnUseBtn()
    {
        ItemSO ItemData = selectedItem.itemData;

        if (ItemData.itemType == ItemType.Consume)
        {
            for (int i = 0; i < ItemData.ConsumeData.Count; i++)
            {
                switch (ItemData.ConsumeData[i].consumeType)
                {
                    case ConsumeType.Health:
                        condition.Heal(ItemData.ConsumeData[i].itemEffectValue);
                        break;
                    case ConsumeType.Stamina:
                        condition.GetStamina(ItemData.ConsumeData[i].itemEffectValue);
                        break;
                    case ConsumeType.Hunger:
                        condition.Eat(ItemData.ConsumeData[i].itemEffectValue);
                        break;
                    case ConsumeType.Thirst:
                        condition.Drink(ItemData.ConsumeData[i].itemEffectValue);
                        break;
                    case ConsumeType.Temperature:
                        condition.GetWarm(ItemData.ConsumeData[i].itemEffectValue);
                        break;
                }
            }
        }
        RemoveSelectedItem();
        SelectItem(selectedItemIndex);
    }

    public void OnEquipBtn()
    {
        if (CharacterManager.Instance.player.equip.curEquip != null)
        {
            return;
        }
        else
        {
            selectedItem.outline.enabled = true;
            selectedItem.equipped = true;
            CharacterManager.Instance.player.equip.newEquip(selectedItem.itemData);
            UpdateInventory();
        }
        SelectItem(selectedItemIndex);
    }

    public void OnUnEquipBtn()
    {
        selectedItem.outline.enabled = false;
        selectedItem.equipped = false;
        CharacterManager.Instance.player.equip.unEquip();
        UpdateInventory();
        SelectItem(selectedItemIndex);
    }

    public void OnBuildBtn()
    {
        if (WorldLevelManager.Instance.buildingSystem.LoadBuildObject(selectedItem.itemData.identifier))
        {
            CharacterManager.Instance.player.controller.ToggleInventory?.Invoke();
            CharacterManager.Instance.player.controller.isBuilding = true;
        }
    }

    public void FillWithLiquid(ItemSO itemData)
    {
        int stack = 0;

        for(int i  = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData != null && slots[i].itemData.identifier == ItemIdentifier.WaterBottle_Empty)
            {
                stack += slots[i].itemCount;
                slots[i].itemCount = 0;
                slots[i].Clear();
            }
        }
        Debug.Log(stack);
        List<ItemStack> water_Full = new List<ItemStack>();
        water_Full.Add(new ItemStack(itemData, stack));
        AddItem(water_Full);
    }
}

