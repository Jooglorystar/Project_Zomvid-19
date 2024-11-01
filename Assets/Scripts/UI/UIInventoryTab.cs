using TMPro;
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
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDesc;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
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
        dropButton.SetActive(false);
    }

    private void AddItem()
    {
        // 중복 확인

        // 빈 슬롯 확인

        // 빈 슬롯 있음

        // 빈 슬롯 없음
    }
}
