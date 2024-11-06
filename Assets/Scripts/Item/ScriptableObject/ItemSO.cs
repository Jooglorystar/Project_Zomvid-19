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

// ������ �ĺ� �ڵ�
public enum ItemIdentifier
{
    //���� 100��
    Axe = 100,
    Bat,
    Knife,
    Pistol,
    Hand,
    Pickaxe,

    //�����ڿ� 1000��
    Wood_Log = 1000,
    Stone,

    //�����ڿ� 2000��
    Pork_raw = 2000,
    Chicken,
    Steak,
    WaterBottle_Full,
    WaterBottle_Empty,
    Apple,

    //�����ڿ� = 3000��
    Wood_Plank = 3000,
    Foundation,
    Pillar,
    Fence,
    Campfire,

    //�Ǽ��ڿ� = 4000��
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
