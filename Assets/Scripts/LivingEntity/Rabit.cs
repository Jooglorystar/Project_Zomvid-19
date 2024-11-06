using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabit : LivingEntity
{
    private Vector3 escapeDestination;
    private bool isEscaping = false;

    protected override void DetectPlayer()
    {
        SetState(AIState.Escape);
    }
    protected override void EscapeUpdate() 
    {
        if (playerDistance < data.detectDistance)
        {
            if (!isEscaping)
            {
                escapeDestination = GetAdjustedWanderLocation();
                agent.SetDestination(escapeDestination);
                isEscaping = true;
            }

            //목표한지점까지 0.5가 남았다면 그리고 agent가 목표지점까지 경로계산을 마쳤다면.
            if (agent.remainingDistance < 1.0f && !agent.pathPending)
            {
                isEscaping = false;
            }
        }
        else
        {
            if (!isEscaping) return;
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
            isEscaping = false;
        }
    }

    private Vector3 GetAdjustedWanderLocation()
    {
        Vector3 wanderLocation = GetWanderLocation();
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;
        Vector3 directionToWander = wanderLocation - transform.position;

        if (Vector3.Dot(directionToPlayer.normalized, directionToWander.normalized) > 0)
        {
            wanderLocation = transform.position - directionToPlayer.normalized * data.maxWanderDistance;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(wanderLocation, out hit, data.maxWanderDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }

}
