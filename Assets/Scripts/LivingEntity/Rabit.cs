using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabit : LivingEntity
{
    private Vector3 escapeDestination; // ���� ���� ���� ��ǥ ����
    private bool isEscaping = false; // ���� �� ����

    protected override void DetectPlayer()
    {
        SetState(AIState.Escape);
    }
    protected override void EscapeUpdate() 
    {
        //�÷��̾ �����Ÿ����� �ִٸ�
        if (playerDistance < data.detectDistance)
        {
            if (!isEscaping)
            {
                escapeDestination = GetAdjustedWanderLocation();
                agent.SetDestination(escapeDestination);
                isEscaping = true; // ���� ���·� ��ȯ
            }

            if (agent.remainingDistance < 0.1f)
            {
                isEscaping = false;
            }
        }
        //�÷��̾ �����Ÿ� ���� ���ٸ�
        else
        {
            if (!isEscaping) return; //�������� �ƴ϶�� ����
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
            isEscaping = false;
        }
    }

    private Vector3 GetAdjustedWanderLocation()
    {
        // 1. GetWanderLocation()�� ȣ���Ͽ� �⺻ ���� ��ġ ��������
        Vector3 wanderLocation = GetWanderLocation();

        // 2. �÷��̾���� ���� ���� ���
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;

        // 3. wanderLocation�� �÷��̾�� �ݴ� �������� Ȯ���ϰ�, �ݴ� ������ �ƴ� ��� ����
        Vector3 directionToWander = wanderLocation - transform.position;

        // ���� ����: �÷��̾�� ���� ������ ��� �ݴ� �������� ����
        if (Vector3.Dot(directionToPlayer.normalized, directionToWander.normalized) > 0)
        {
            // �ݴ� �������� ����
            wanderLocation = transform.position - directionToPlayer.normalized * data.maxWanderDistance;
        }

        // 4. NavMesh ���� ��ȿ�� ��ġ�� ����
        NavMeshHit hit;
        if (NavMesh.SamplePosition(wanderLocation, out hit, data.maxWanderDistance, NavMesh.AllAreas))
        {
            return hit.position; // ��ȿ�� ��ġ ��ȯ
        }

        // ��ȿ�� ��ġ�� ������ ���� ��ġ ��ȯ
        return transform.position;
    }

}
