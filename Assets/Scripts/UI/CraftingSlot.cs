using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public ItemSO itemData;
    [SerializeField] private Image slotIcon;

    public UICraftingTab craftingTab;

    public int index;

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

    public void OnClickButton()
    {
        craftingTab.SelectItem(index);
    }
}