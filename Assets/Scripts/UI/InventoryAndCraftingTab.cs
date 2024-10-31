using UnityEngine;
using UnityEngine.InputSystem;

public enum InventoryState
{
    Inventory,
    Crafting
}

public enum PageState
{
    Page1,
    Page2
}

public class InventoryAndCraftingTab : MonoBehaviour
{
    [SerializeField] private GameObject inventoryTab;
    [SerializeField] private GameObject craftingTab;

    [SerializeField] private GameObject NextPageButton;
    [SerializeField] private GameObject PreviousPageButton;

    [SerializeField] private GameObject Inventory1Page;
    [SerializeField] private GameObject Inventory2Page;
    [SerializeField] private GameObject Crafting1Page;
    [SerializeField] private GameObject Crafting2Page;


    static public InventoryState State;

    public void Start()
    {
        PreviousPageButton.SetActive(false);
        Inventory2Page.SetActive(false);
        Crafting2Page.SetActive(false);
        State = InventoryState.Inventory;
    }

    public void OnInventoryUI(InputAction.CallbackContext context)
    {

    }

    // 인벤토리 탭 버튼
    public void OnInventoryTab()
    {
        if (State == InventoryState.Crafting)
        {
            craftingTab.SetActive(false);
            inventoryTab.SetActive(true);
            State = InventoryState.Inventory;
        }
    }

    // 크래프팅 탭 버튼
    public void OnCraftingTab()
    {
        if (State == InventoryState.Inventory)
        {
            inventoryTab.SetActive(false);
            craftingTab.SetActive(true);
            State = InventoryState.Crafting;
        }
    }

    // 2페이지로 이동
    public void GoNextPage()
    {
        if (State == InventoryState.Inventory)
        {
            Inventory2Page.SetActive(true);
            Inventory1Page.SetActive(false);
        }
        else if (State == InventoryState.Crafting)
        {
            Crafting2Page.SetActive(true);
            Crafting1Page.SetActive(false);
        }

        PreviousPageButton.SetActive(true);
        NextPageButton.SetActive(false);
    }

    // 1페이지로 이동
    public void GoPreviousPage()
    {
        if (State == InventoryState.Inventory)
        {
            Inventory2Page.SetActive(false);
            Inventory1Page.SetActive(true);
        }
        else if (State == InventoryState.Crafting)
        {
            Crafting2Page.SetActive(false);
            Crafting1Page.SetActive(true);
        }

        NextPageButton.SetActive(true);
        PreviousPageButton.SetActive(false);
    }
}
