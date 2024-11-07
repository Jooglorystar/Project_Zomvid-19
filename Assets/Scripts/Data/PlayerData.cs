using UnityEngine;

public class PlayerData : EntityData
{
    [Header("Status")]
    public float maxStamina;
    public float maxHunger;
    public float maxThirst;

    [Header("Move")]
    public float jumpPower;
    public float jumpStamina;
    public float mouseSensitivity;
}