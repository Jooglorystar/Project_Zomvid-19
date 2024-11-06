using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawnZone : MonoBehaviour
{
    private SpawnManager spawnManager;
    private Transform playerTransform;
    private List<GameObject> SpawnZombies = new List<GameObject>();
    private float despawnDistance = 15f;    //비활성화 할 거리
    private Coroutine deactivateZombiesCoroutine;
    private int spawnCount = 5;
    private void Start()
    {
        Debug.Log("RabitSpawnZone시작");
        spawnManager = GetComponent<SpawnManager>();
        playerTransform = CharacterManager.Instance.player.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject zombie = spawnManager.SpawnEnemies(transform.position, PoolObject.zombie);//이 위치에서 스폰
                SpawnZombies.Add(zombie);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (deactivateZombiesCoroutine != null)
            {
                StopCoroutine(deactivateZombiesCoroutine);
            }
            deactivateZombiesCoroutine = StartCoroutine(DeactivateZombiesCoroutine());
        }
    }

    private IEnumerator DeactivateZombiesCoroutine()
    {
        for(int i = 1; i < SpawnZombies.Count; i++)
        {
            if (SpawnZombies[i] != null)
            {
                float distance = Vector3.Distance(playerTransform.position, SpawnZombies[i].transform.position);
                if (distance > despawnDistance)
                {
                    SpawnZombies[i].SetActive(false); // 거리가 멀어지면 비활성화
                }
            }
            yield return new WaitForSeconds(1f);
        }
        deactivateZombiesCoroutine = null;
    }
}
