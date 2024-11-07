using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public ItemSO itemSO;
    public int stack;

    public ItemStack(ItemSO itemSO, int stack)
    {
        this.itemSO = itemSO;
        this.stack = stack;
    }
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemStack itemStack;

    public void OnInteraction()
    {
        List<ItemStack> itemStacks = new();
        itemStacks.Add(new ItemStack (itemStack.itemSO, itemStack.stack));
        CharacterManager.Instance.player.uiInventoryTab.AddItem(itemStacks);
        //Destroy(gameObject);
    }

    public string GetInteractPromptName()
    {
        return itemStack.itemSO.name;
    }

    public string GetInteractPromptDescription()
    {
        return itemStack.itemSO.itemDesc;
    }
}
