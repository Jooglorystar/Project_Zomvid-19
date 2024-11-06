using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabit : LivingEntity
{
    private Vector3 escapeDestination; // 현재 도망 중인 목표 지점
    private bool isEscaping = false; // 도망 중 여부

    protected override void DetectPlayer()
    {
        SetState(AIState.Escape);
    }
    protected override void EscapeUpdate() 
    {
        //플레이어가 감지거리내에 있다면
        if (playerDistance < data.detectDistance)
        {
            if (!isEscaping)
            {
                escapeDestination = GetAdjustedWanderLocation();
                agent.SetDestination(escapeDestination);
                isEscaping = true; // 도망 상태로 전환
            }

            if (agent.remainingDistance < 0.1f)
            {
                isEscaping = false;
            }
        }
        //플레이어가 감지거리 내에 없다면
        else
        {
            if (!isEscaping) return; //도망중이 아니라면 리턴
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
            isEscaping = false;
        }
    }

    private Vector3 GetAdjustedWanderLocation()
    {
        // 1. GetWanderLocation()을 호출하여 기본 랜덤 위치 가져오기
        Vector3 wanderLocation = GetWanderLocation();

        // 2. 플레이어와의 방향 차이 계산
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;

        // 3. wanderLocation이 플레이어와 반대 방향인지 확인하고, 반대 방향이 아닐 경우 조정
        Vector3 directionToWander = wanderLocation - transform.position;

        // 방향 조정: 플레이어와 같은 방향일 경우 반대 방향으로 보정
        if (Vector3.Dot(directionToPlayer.normalized, directionToWander.normalized) > 0)
        {
            // 반대 방향으로 수정
            wanderLocation = transform.position - directionToPlayer.normalized * data.maxWanderDistance;
        }

        // 4. NavMesh 위의 유효한 위치로 보정
        NavMeshHit hit;
        if (NavMesh.SamplePosition(wanderLocation, out hit, data.maxWanderDistance, NavMesh.AllAreas))
        {
            return hit.position; // 유효한 위치 반환
        }

        // 유효한 위치가 없으면 현재 위치 반환
        return transform.position;
    }

}
