using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    AttackingFence,
    Escape,
    Die
}

public abstract class LivingEntity : MonoBehaviour, IDamagable
{
    internal LivingEntityData data;
    [Header("AI")]
    protected NavMeshAgent agent;
    protected AIState aiState;

    protected float playerDistance;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private Coroutine coroutine;
    private bool isDead = false;

    [SerializeField] private List<ItemSO> dropOnDeath;

    //테스트용
    public bool isStopped;

    private void Awake()
    {
        data = GetComponent<LivingEntityData>();
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
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        if (isStopped) return;

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
            case AIState.AttackingFence:
                AttackingFenceUpdate();
                break;
            case AIState.Escape:
                EscapeUpdate();
                break;
        }
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
            DetectPlayer();
        }
    }
    protected abstract void DetectPlayer();// ex : SetState(AIState.Attacking)
    protected virtual void AttackingUpdate() { }
    protected virtual void AttackingFenceUpdate() { }
    protected virtual void EscapeUpdate() { }

    public void SetState(AIState state)
    {
        aiState = state;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

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
            case AIState.AttackingFence:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Escape:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Die:
                agent.speed = 0;
                agent.isStopped = true;
                animator.SetTrigger("Dying");
                Invoke("Die", 4.0f);
                break;

        }

        animator.SetFloat("Speed", agent.speed);
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    protected Vector3 GetWanderLocation()
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
    protected bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < data.fieldOfView * 0.5f;
    }

    public virtual void TakeDamage(float damage)
    {
        data.maxHealth -= damage;

        if (data.maxHealth <= 0 && !isDead)
        {
            isDead = true;
            SetState(AIState.Die);
        }

        //데미지 효과
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DamageFlash());
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
    void Die()
    {
        //아이템 드롭부분
        for (int i = 0; i < dropOnDeath.Count; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        gameObject.SetActive(false);   //data.gameObject를 파괴하는 이유가..? 죽을 때 모션이후에 죽고싶으면 리지드바디나 다른 컴포넌트들을 제거해야할지도
    }

    void OnDisable()
    {
        CancelInvoke("WanderToNewLocation");
    }
}
