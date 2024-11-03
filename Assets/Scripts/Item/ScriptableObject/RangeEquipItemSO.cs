using UnityEngine;

[CreateAssetMenu(fileName = "EquipItemSO", menuName = "RangeDefaultSO", order = 2)]

public class RangeEquipItemSO : ItemSO
{
    [Header("Melee Equipment")]
    public float damage;
    public float attackDistance;    //총알 사거리
    public float attackRate;        //연사속도
    public float useStamina;
    public ToolType toolType;
    public GameObject rangePrefab;
}