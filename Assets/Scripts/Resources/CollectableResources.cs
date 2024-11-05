using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableResources : MonoBehaviour, IInteractable
{
    [SerializeField] private string resourceName;
    [Multiline][SerializeField] private string description;

    [SerializeField] private List<ItemStack> itemStacks;


    public void OnInteraction()
    {
        CharacterManager.Instance.player.uiInventoryTab.AddItem(itemStacks);
        Destroy(gameObject);
    }

    public string GetInteractPromptName()
    {
        return resourceName;
    }

    public string GetInteractPromptDescription()
    {
        return description;
    }
}
