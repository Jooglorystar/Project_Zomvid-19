using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GatheringResource : MonoBehaviour
{
    [SerializeField] private float durability = 100f; // 최대 체력
    private float curDurability;

    //[SerializeField] private ItemData[] ListEarlyGatherings; 체력이 깎일 때마다 비율로 반환
    //[SerializeField] private ItemData[] ListEndGatherings; 체력이 전부 깎였을 때 한번에 반환

    [SerializeField] private bool regenerative = true; //재생됨
    private bool depleted;
    private float depletedTime;

}
