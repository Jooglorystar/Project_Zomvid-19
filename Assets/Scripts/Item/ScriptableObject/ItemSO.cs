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

// 아이템 식별 코드
public enum ItemIdentifier
{
    //무기 100번
    Axe = 100,
    Bat,
    Knife,
    Pistol,
    Hand,
    Pickaxe,

    //기초자원 1000번
    Wood_Log = 1000,
    Stone,

    //음식자원 2000번
    Pork_raw = 2000,
    Chicken,
    Steak,
    WaterBottle_Full,
    WaterBottle_Empty,
    Apple,

    //가공자원 = 3000번
    Wood_Plank = 3000,
    Foundation,
    Pillar,
    Fence,
    Campfire,

    //건설자원 = 4000번
    Build_Foundation = 4000,
    Build_Wall,
    Build_Pillar,
    Build_Fence,
    Build_Campfire,

    TestRsource1 = 10000, TestRsource2 = 10001
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
    public ItemIdentifier identifier;
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
