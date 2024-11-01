﻿using UnityEngine;

[CreateAssetMenu(fileName = "EquipItemSO", menuName = "MeleeDefaultSO", order = 1)]
public class MeleeEquipItemSO : ItemSO
{
    [Header("Melee Equipment")]
    public float damage;
    public float attackDistance;
    public float attackRate;
    public float useStamina;
}