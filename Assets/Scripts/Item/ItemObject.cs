using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO itemData;
    public int stack;


    public void OnInteraction()
    {
        // TODO : 인벤토리 호출 -> 아이템 소매넣기
        CharacterManager.Instance.player.itemData = itemData;
        Debug.Log(itemData.itemName);
        CharacterManager.Instance.player.AddItem?.Invoke();
        Destroy(gameObject);
    }

    public string GetInteractPromptName()
    {
        return itemData.name;
    }

    public string GetInteractPromptDescription()
    {
        return itemData.itemDesc;
    }
}
