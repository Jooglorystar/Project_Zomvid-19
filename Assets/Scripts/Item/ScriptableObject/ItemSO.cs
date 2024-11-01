using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equip,
    Consume,
    Resource,
    Build
}

public enum ConsumeType
{
    Health,
    Stamina,
    Hunger,
    Thirst,
    Temperature
}

[System.Serializable]
public class ConsumeData
{
    public ConsumeType consumeType;
    public float itemEffectValue;
}

[System.Serializable]
public class ItemInRecipe
{
    public ItemSO item;
    public int itemCount;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemDefaultSO", order = 0)]
public class ItemSO : ScriptableObject
{
    [Header("Item Infomation")]
    public string itemName;
    public string itemDesc;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stack")]
    public bool canStack;
    public int MaxStackSize;

    [Header("Item Stat")]
    public List<ConsumeData> ConsumeData;

    [Header("Crafting")]
    public bool canCrafting;
    public List<ItemInRecipe> itemMaterials;
}
