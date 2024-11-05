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
        Debug.Log($"����: {aiState}");
        //�÷��̾ �տ��ְ� �����Ÿ����� �ִٸ�
        if (playerDistance < data.detectDistance)
        {
            if (!isEscaping)
            {
                Debug.Log($"if����: {aiState}");
                Debug.Log("����");
                escapeDestination = GetAdjustedWanderLocation();
                agent.SetDestination(escapeDestination);
                isEscaping = true; // ���� ���·� ��ȯ
                Debug.Log($"���� ��ġ: {escapeDestination}");
            }

            if (agent.remainingDistance < 0.1f)
            {
                isEscaping = false;
            }
        }
        //�÷��̾ �þ� ���� ���� �����Ÿ����� ���ٸ�
        else
        {
            Debug.Log($"else����: {aiState}");
            if (!isEscaping) return;
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
