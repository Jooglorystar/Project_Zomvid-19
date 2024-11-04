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
        //플레이어가 앞에있고 감지거리내에 있다면
        if (playerDistance < data.detectDistance && IsPlayerInFieldOfView())
        {
            //도망간다
            agent.isStopped = false;
            agent.SetDestination(GetWanderLocation());
        }
        //플레이어가 시야 내에 없고 감지거리내에 없다면
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(AIState.Wandering);
        }
    }
}
