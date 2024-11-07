using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Terrain terrain;

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

        float groundHeight = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;

        spawnPos.y = groundHeight;

        // Object Pool에서 좀비나 토끼 가져오기
        GameObject entity = GameManager.Instance.objectPool.GetFromPool(poolObject);
        entity.transform.position = spawnPos;
        entity.SetActive(true);

        NavMeshAgent agent = entity.GetComponent<NavMeshAgent>();

        return entity;
    }
}
