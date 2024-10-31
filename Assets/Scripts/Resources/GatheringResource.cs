using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GatheringResource : MonoBehaviour
{
    [SerializeField] private float durability = 100f; // �ִ� ü��
    private float curDurability;

    //[SerializeField] private ItemData[] ListEarlyGatherings; ü���� ���� ������ ������ ��ȯ
    //[SerializeField] private ItemData[] ListEndGatherings; ü���� ���� ���� �� �ѹ��� ��ȯ

    [SerializeField] private bool regenerative = true; //�����
    private bool depleted;
    private float depletedTime;

}
