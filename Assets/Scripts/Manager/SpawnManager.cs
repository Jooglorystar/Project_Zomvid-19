using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("SpawnManager시작");
    }
    [SerializeField]
    private GameObject objectPool;

    public GameObject SpawnEnemies(Vector3 spawnLocation, PoolObject poolObject)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
        Vector3 spawnPos = spawnLocation + randomOffset;
        if (Physics.Raycast(spawnPos + Vector3.up * 10, Vector3.down, out RaycastHit hitInfo, 20f))
        {
            spawnPos.y = hitInfo.point.y; // 바닥에 위치를 맞춤
        }
        else
        {
            spawnPos.y = spawnLocation.y; // 레이캐스트 실패 시 기본 위치
        }

        // Object Pool에서 좀비나 토끼 가져오기
        GameObject entity = GameManager.Instance.objectPool.GetFromPool(poolObject);
        entity.transform.position = spawnPos;
        entity.SetActive(true);

        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();

        return entity;
    }
}
