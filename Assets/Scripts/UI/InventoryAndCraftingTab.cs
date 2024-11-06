using UnityEngine;
using UnityEngine.InputSystem;

public enum InventoryState
{
    Inventory,
    Crafting
}

public class InventoryAndCraftingTab : MonoBehaviour
{
    [SerializeField] private GameObject wholeTab;

    [SerializeField] private GameObject inventoryTab;
    [SerializeField] private GameObject craftingTab;

    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;

    [SerializeField] private GameObject inventory1Page;
    [SerializeField] private GameObject inventory2Page;
    [SerializeField] private GameObject crafting1Page;
    [SerializeField] private GameObject crafting2Page;

    private InventoryState _state;

    public void Start()
    {
        ResetTap();

        _state = InventoryState.Inventory;
        wholeTab.SetActive(false);

        CharacterManager.Instance.player.controller.ToggleInventory = ToggleInventoryUI;
    }

    public void OnInventoryUI(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            ToggleInventoryUI();
        }
    }

    // 창을 여닿는 메서드
    private void ToggleInventoryUI()
    {
        ResetTap();

        if (isOpen())
        {
            wholeTab.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            // 다시 품
            //CharacterManager.Instance.player.controller.canMove = true;
            //CharacterManager.Instance.player.controller.canLook = true;

        }
        else
        {
            wholeTab.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            // 인벤토리 오픈 시 움직임 및 회전을 막음
            //CharacterManager.Instance.player.controller.canMove = false;
            //CharacterManager.Instance.player.controller.canLook = false;

            if (CharacterManager.Instance.player.controller.isBuilding) // 건설 종료
            {
                CharacterManager.Instance.player.controller.isBuilding = false;
                WorldLevelManager.Instance.buildingSystem.ExitBuild();
            }
        }
    }

    private bool isOpen()
    {
        return wholeTab.activeInHierarchy;
    }


    private void ResetTap()
    {
        previousPageButton.SetActive(false);
        inventory2Page.SetActive(false);
        crafting2Page.SetActive(false);
        GoPreviousPage();
    }

    // 인벤토리 탭 버튼
    public void OnInventoryTab()
    {
        if (_state == InventoryState.Crafting)
        {
            craftingTab.SetActive(false);
            inventoryTab.SetActive(true);
            ResetTap();
            _state = InventoryState.Inventory;
        }
    }

    // 크래프팅 탭 버튼
    public void OnCraftingTab()
    {
        if (_state == InventoryState.Inventory)
        {
            inventoryTab.SetActive(false);
            craftingTab.SetActive(true);
            ResetTap();
            _state = InventoryState.Crafting;
        }
    }

    // 2페이지로 이동
    public void GoNextPage()
    {
        if (_state == InventoryState.Inventory)
        {
            inventory1Page.SetActive(false);
            inventory2Page.SetActive(true);
        }
        else if (_state == InventoryState.Crafting)
        {
            crafting1Page.SetActive(false);
            crafting2Page.SetActive(true);
        }

        nextPageButton.SetActive(false);
        previousPageButton.SetActive(true);
    }

    // 1페이지로 이동
    public void GoPreviousPage()
    {
        if (_state == InventoryState.Inventory)
        {
            inventory1Page.SetActive(true);
            inventory2Page.SetActive(false);
        }
        else if (_state == InventoryState.Crafting)
        {
            crafting1Page.SetActive(true);
            crafting2Page.SetActive(false);
        }

        nextPageButton.SetActive(true);
        previousPageButton.SetActive(false);
    }
}
