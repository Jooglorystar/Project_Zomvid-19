using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICraftingTab : MonoBehaviour
{
    public CraftingSlot[] slots;

    public Transform Page1;
    public Transform Page2;

    [Header("Select Item")]
    private CraftingSlot selectedItem;
    private int selectedItemIndex;

    public TextMeshProUGUI craftingItemNameText;
    public TextMeshProUGUI craftingItemDescText;
    public TextMeshProUGUI materialNameText;
    public TextMeshProUGUI materialCountText;

    public GameObject craftButton;

    public UIInventoryTab inventoryTab;

    [Header("Crafting Value")]
    private int[] hasMaterialCounts;

    private void Start()
    {
        slots = new CraftingSlot[Page1.childCount + Page2.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Page1.childCount)
            {
                slots[i] = Page1.GetChild(i).GetComponent<CraftingSlot>();
            }
            else if (i >= Page1.childCount)
            {
                slots[i] = Page2.GetChild(i - Page1.childCount).GetComponent<CraftingSlot>();
            }

            slots[i].index = i;
            slots[i].craftingTab = this;
        }
        ClearCraftingItemWindow();
    }

    // 창 초기화
    private void ClearCraftingItemWindow()
    {
        craftingItemNameText.text = string.Empty;
        craftingItemDescText.text = string.Empty;
        materialNameText.text = string.Empty;
        materialCountText.text = string.Empty;

        craftButton.SetActive(false);
    }

    // 선택 아이템 표시
    public void SelectItem(int index)
    {
        if (slots[index].itemData == null)
        {
            ClearCraftingItemWindow();
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        craftingItemNameText.text = selectedItem.itemData.itemName;
        craftingItemDescText.text = selectedItem.itemData.itemDesc;

        materialNameText.text = string.Empty;
        materialCountText.text = string.Empty;

        hasMaterialCounts = new int[selectedItem.itemData.itemMaterials.Count];

        // 재료 표시
        for (int i = 0; i < selectedItem.itemData.itemMaterials.Count; i++)
        {
            materialNameText.text += selectedItem.itemData.itemMaterials[i].item.itemName.ToString() + "\n";
            

            int needCount = selectedItem.itemData.itemMaterials[i].itemCount;
            int hasCount = 0;

            // 레시피와 같은 재료 찾기
            for (int j = 0; j < inventoryTab.slots.Length; j++)
            {
                if (selectedItem.itemData.itemMaterials[i].item == inventoryTab.slots[j].itemData)
                {
                    hasCount = inventoryTab.slots[j].itemCount;
                    break;
                }
            }
            hasMaterialCounts[i] = hasCount;
            string hasCountAndNeedCount = $"{hasCount} / {needCount}";

            // 미달된 아이템은 붉은 표시
            if (hasCount < needCount)
            {
                hasCountAndNeedCount = $"<color=#FF0000>{hasCountAndNeedCount}</color>";
            }
            // 충족된 아이템은 녹색 표시
            else
            {
                hasCountAndNeedCount = $"<color=#00FF00>{hasCountAndNeedCount}</color>";
            }

            materialCountText.text += hasCountAndNeedCount + "\n";
        }

        craftButton.SetActive(true);
    }

    // 제작 버튼
    public void OnCraftButton()
    {
        if (HasAllMaterial())
        {
            Debug.Log("제작 완료");
        }
        else
        {
            Debug.Log("재료 부족");
        }
    }


    // 선택된 아이템 재료 수 체크 메서드
    private bool HasAllMaterial()
    {
        for (int i = 0; i < hasMaterialCounts.Length; i++)
        {
            if(hasMaterialCounts[i] < selectedItem.itemData.itemMaterials[i].itemCount)
            {
                Debug.Log($"{hasMaterialCounts[i]}/{selectedItem.itemData.itemMaterials[i].itemCount}\n{selectedItem.itemData.itemMaterials[i].item.itemName}이 부족");
                return false;
            }
            else
            {
                continue;
            }
        }
        return true;
    }
}