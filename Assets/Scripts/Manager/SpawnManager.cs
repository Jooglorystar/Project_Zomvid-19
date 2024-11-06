using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("SpawnManager����");
    }
    [SerializeField]
    private GameObject objectPool;

    public GameObject SpawnEnemies(Vector3 spawnLocation, PoolObject poolObject)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Vector3 spawnPos = spawnLocation + randomOffset;
        if (Physics.Raycast(spawnPos + Vector3.up * 10, Vector3.down, out RaycastHit hitInfo, 20f))
        {
            spawnPos.y = hitInfo.point.y; // �ٴڿ� ��ġ�� ����
        }
        else
        {
            spawnPos.y = spawnLocation.y; // ����ĳ��Ʈ ���� �� �⺻ ��ġ
        }

        // Object Pool���� ���� �䳢 ��������
        GameObject entity = GameManager.Instance.objectPool.GetFromPool(poolObject);
        entity.transform.position = spawnPos;
        entity.SetActive(true);

        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();

        return entity;
    }
}
