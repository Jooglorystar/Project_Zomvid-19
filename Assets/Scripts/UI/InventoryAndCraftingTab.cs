using UnityEngine;

public enum InventoryState
{
    Inventory,
    Crafting
}

public class InventoryAndCraftingTab : MonoBehaviour
{
    [SerializeField] private GameObject inventoryTab;
    [SerializeField] private GameObject craftingTab;

    static public InventoryState State;

    public void Start()
    {
        State = InventoryState.Inventory;
    }
    public void OnInventoryTab()
    {
        if (inventoryTab.activeInHierarchy) return;

        craftingTab.SetActive(false);
        inventoryTab.SetActive(true);
        State = InventoryState.Inventory;
    }

    public void OnCraftingTab()
    {
        if (craftingTab.activeInHierarchy) return;

        inventoryTab.SetActive(false);
        craftingTab.SetActive(true);
        State = InventoryState.Crafting;
    }
}
