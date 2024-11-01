using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking
}

public class Zombie : MonoBehaviour, IDamagable
{
    internal ZombieData data;

    [Header("AI")]
    private NavMeshAgent agent;
    private AIState aiState;

    private float lastAttackTime;
    private float playerDistance;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private Coroutine coroutine;

    private void Awake()
    {
        data = GetComponent<ZombieData>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }
    void Update()
    {
        playerDistance = Vector2.Distance(transform.position, CharacterManager.Instance.player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;

        }
    }


    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = data.walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = data.walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;

        }

        animator.speed = agent.speed / data.walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        }

        if (playerDistance < data.detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        float wanderRagne = Random.Range(data.minWanderDistance, data.maxWanderDistance);
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * wanderRagne), out hit, data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < data.detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    void AttackingUpdate()
    {
        if (playerDistance < data.attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > data.attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.player.controller.GetComponent<IDamagable>().TakeDamage(data.basicATK);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            //공격범위안에는 있지만
            if (playerDistance < data.detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < data.fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        data.maxHealth -= damage;
        if (data.maxHealth <= 0)
        {
            Die();
        }

        //데미지 효과
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DamageFlash());
    }

    void Die()
    {
        ////아이템 드롭부분
        //for (int i = 0; i < dropOnDeath.Length; i++)
        //{
        //    Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        //}

        Destroy(data.gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int x = 0; x < meshRenderers.Length; x++)
        {
            meshRenderers[x].material.color = Color.white;
        }
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
