using System.Collections.Generic;
using UnityEngine;


public class InventoryAndCraftingPage : MonoBehaviour
{
    [SerializeField] private GameObject NextPageButton;
    [SerializeField] private GameObject PreviousPageButton;

    [SerializeField] private GameObject Inventory1Page;
    [SerializeField] private GameObject Inventory2Page;
    [SerializeField] private GameObject Crafting1Page;
    [SerializeField] private GameObject Crafting2Page;

    public void Start()
    {
        PreviousPageButton.SetActive(false);
        Inventory2Page.SetActive(false);
        Crafting2Page.SetActive(false);
    }

    public void GoNextPage()
    {
        if(InventoryAndCraftingTab.State == InventoryState.Inventory)
        {
            Inventory2Page.SetActive(true);
            Inventory1Page.SetActive(false);
        }
        else if(InventoryAndCraftingTab.State == InventoryState.Crafting)
        {
            Crafting2Page.SetActive(true);
            Crafting1Page.SetActive(false);
        }

        PreviousPageButton.SetActive(true);
        NextPageButton.SetActive(false);
    }

    public void GoPreviousPage()
    {
        if (InventoryAndCraftingTab.State == InventoryState.Inventory)
        {
            Inventory2Page.SetActive(false);
            Inventory1Page.SetActive(true);
        }
        else if (InventoryAndCraftingTab.State == InventoryState.Crafting)
        {
            Crafting2Page.SetActive(false);
            Crafting1Page.SetActive(true);
        }

        NextPageButton.SetActive(true);
        PreviousPageButton.SetActive(false);
    }
}