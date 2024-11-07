using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawnZone : MonoBehaviour
{
    public float despawnDistance = 15f;    //��Ȱ��ȭ �� �Ÿ�
    public int spawnCount = 5;
    
    private SpawnManager spawnManager;
    private Transform playerTransform;
    private List<GameObject> SpawnZombies = new List<GameObject>();
    private Coroutine deactivateZombiesCoroutine;

    private void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        playerTransform = CharacterManager.Instance.player.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < spawnCount; i++)
            {
                //j�� 0, 1, 2, 3
                int j = Random.Range(0, 4);
                GameObject zombie;

                //25% Ȯ���� ��ȣ������ ��ȯ
                if (j == 0)
                {
                    zombie = spawnManager.SpawnEnemies(transform.position, PoolObject.zombie_Nurse);//�� ��ġ���� ����
                }
                else
                {
                    zombie = spawnManager.SpawnEnemies(transform.position, PoolObject.zombie);//�� ��ġ���� ����
                }
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
        for(int i = 0; i < SpawnZombies.Count; i++)
        {
            if (SpawnZombies[i] != null)
            {
                float distance = Vector3.Distance(playerTransform.position, SpawnZombies[i].transform.position);
                if (distance > despawnDistance)
                {
                    SpawnZombies[i].SetActive(false); // �Ÿ��� �־����� ��Ȱ��ȭ
                }
            }
            yield return new WaitForSeconds(1f);
        }
        deactivateZombiesCoroutine = null;
    }
}
