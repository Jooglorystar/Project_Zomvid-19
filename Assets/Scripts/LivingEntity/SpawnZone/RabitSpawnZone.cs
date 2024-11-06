using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabitSpawnZone : MonoBehaviour
{
    private SpawnManager spawnManager;
    private Transform playerTransform;
    private List<GameObject> SpawnRabbits = new List<GameObject>();
    private float despawnDistance = 15f;    //비활성화 할 거리
    private Coroutine deactivateRabbitsCoroutine;
    private int spawnCount = 5;
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
                GameObject rabbit = spawnManager.SpawnEnemies(transform.position, PoolObject.rabit);//이 위치에서 스폰
                SpawnRabbits.Add(rabbit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (deactivateRabbitsCoroutine != null)
            {
                StopCoroutine(deactivateRabbitsCoroutine);
            }
            deactivateRabbitsCoroutine = StartCoroutine(DeactivateRabbitsCoroutine());
        }
    }

    private IEnumerator DeactivateRabbitsCoroutine()
    {
        for (int i = 0; i < SpawnRabbits.Count; i++)
        {
            if (SpawnRabbits[i] != null)
            {
                float distance = Vector3.Distance(playerTransform.position, SpawnRabbits[i].transform.position);
                if (distance > despawnDistance)
                {
                    SpawnRabbits[i].SetActive(false); // 거리가 멀어지면 비활성화
                }
            }
            yield return new WaitForSeconds(1f);
        }
        deactivateRabbitsCoroutine = null; 
    }

    private void OnDrawGizmos()
    {
        // 원하는 색 설정
        Gizmos.color = new Color(0, 1, 0, 0.3f); // 예: 초록색, 불투명도 30%

        // BoxCollider의 사이즈와 위치에 맞춰 사각형 Gizmo를 그림
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
    }
}
