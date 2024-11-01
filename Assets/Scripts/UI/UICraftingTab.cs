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

    public TextMeshProUGUI craftingItemName;
    public TextMeshProUGUI craftingItemDesc;
    public TextMeshProUGUI materialName;
    public TextMeshProUGUI materialCount;

    public GameObject craftButton;

    public UIInventoryTab inventoryTab;

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
        craftingItemName.text = string.Empty;
        craftingItemDesc.text = string.Empty;
        materialName.text = string.Empty;
        materialCount.text = string.Empty;

        craftButton.SetActive(false);
    }

    // 선택 아이템 표시
    public void SelectItem(int index)
    {
        if (slots[index].itemData == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        craftingItemName.text = selectedItem.itemData.itemName;
        craftingItemDesc.text = selectedItem.itemData.itemDesc;

        materialName.text = string.Empty;
        materialCount.text = string.Empty;

        // 재료 표시
        for (int i = 0; i < selectedItem.itemData.itemMaterials.Count; i++)
        {
            materialName.text += selectedItem.itemData.itemMaterials[i].item.itemName.ToString() + "\n";

            int needCount = selectedItem.itemData.itemMaterials[i].itemCount;
            int hasCount = 0;

            for (int j = 0; j < inventoryTab.slots.Length; j++)
            {
                if (selectedItem.itemData.itemMaterials[i].item == inventoryTab.slots[j].itemData)
                {
                    hasCount = inventoryTab.slots[j].itemCount;
                    break;
                }
            }
            string hasCountAndNeedCount = $"{hasCount} / {needCount}";
            
            // 미달된 아이템은 붉은 표시
            if (hasCount < needCount)
            {
                hasCountAndNeedCount = $"<color=#FF0000>{hasCountAndNeedCount}</color>";
            }

            materialCount.text += hasCountAndNeedCount + "\n";
        }

        craftButton.SetActive(true);
    }

    private void OnCraftButton()
    {

    }
}