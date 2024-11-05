using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabit : LivingEntity<LivingEntityData>, IDamagable
{
    private LivingEntityData livingData;
    protected override void DetectPlayer()
    {
        SetState(AIState.Escape);
    }

    protected override void EscapeUpdate() 
    {
        //�÷��̾ �տ��ְ� �����Ÿ����� �ִٸ�
        if (playerDistance < data.detectDistance && IsPlayerInFieldOfView())
        {
            //��������
            agent.isStopped = false;
            agent.SetDestination(GetWanderLocation());
        }
        //�÷��̾ �þ� ���� ���� �����Ÿ����� ���ٸ�
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
        }
    }
}
