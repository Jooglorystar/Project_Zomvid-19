using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO itemData;
    [SerializeField] private GameObject parent;
    [HideInInspector] public int stack;


    public void InitializeObject(ItemSO _itemData, int _stack, Vector3 dropPosition)
    {


        Instantiate(itemData.dropPrefab, dropPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

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
