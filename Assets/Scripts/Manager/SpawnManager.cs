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
        Vector3 randomOffset = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        Vector3 spawnPos = spawnLocation + randomOffset;

        // Object Pool���� ���� �䳢 ��������
        GameObject entity = GameManager.Instance.objectPool.GetFromPool(poolObject);
        entity.transform.position = spawnPos;
        entity.SetActive(true);

        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.enabled = true; // NavMeshAgent Ȱ��ȭ
        }

        return entity;
    }
}
