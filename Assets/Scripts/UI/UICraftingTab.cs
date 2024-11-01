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

    private void ClearCraftingItemWindow()
    {
        craftingItemName.text = string.Empty;
        craftingItemDesc.text = string.Empty;
        materialName.text = string.Empty;
        materialCount.text = string.Empty;

        craftButton.SetActive(false);
    }

    public void SelectItem(int index)
    {
        if (slots[index].itemData == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        craftingItemName.text = selectedItem.itemData.itemName;
        craftingItemDesc.text = selectedItem.itemData.itemDesc;

        materialName.text = string.Empty;
        materialCount.text = string.Empty;

        for (int i = 0; i < selectedItem.itemData.itemMaterials.Count; i++)
        {
            materialName.text += selectedItem.itemData.itemMaterials[i].item.itemName.ToString() + "\n";
            materialCount.text += selectedItem.itemData.itemMaterials[i].itemCount.ToString() + "\n";
        }

        craftButton.SetActive(true);
    }

    private void OnCraftButton()
    {

    }
}