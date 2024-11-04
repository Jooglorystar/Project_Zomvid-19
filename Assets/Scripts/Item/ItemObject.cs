using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO itemData;
    public int stack;


    public void OnInteraction()
    {
        // TODO : �κ��丮 ȣ�� -> ������ �Ҹųֱ�
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
