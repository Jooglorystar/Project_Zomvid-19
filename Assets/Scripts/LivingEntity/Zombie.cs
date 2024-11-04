using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Zombie : MonoBehaviour, IDamagable
{
    //public InputActionAsset inputActions;
    //private InputActionMap inputMap;

    internal ZombieData data;

    [Header("AI")]
    [SerializeField] private LayerMask fenceMaskLayer;
    private NavMeshAgent agent;
    private AIState aiState;

    private float lastAttackTime;
    private float playerDistance;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private Coroutine coroutine;

    public bool isStopped;
    private IDamagable FenceAttacked;

    private void Awake()
    {
        data = GetComponent<ZombieData>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        //키보드
        //inputMap  = inputActions.actionMaps[0];
        //inputMap.FindAction("Stop") += StopZombie;

        SetState(AIState.Wandering);
    }
    void Update()
    {
        // 1번 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LockCursor();
        }

        // 2번 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnlockCursor();
        }

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
        }
    }

    private void AttackingFenceUpdate()
    {
        if (!(playerDistance < data.attackDistance && IsPlayerInFieldOfView()))
        {
            if (IsFenceInFront())
            {
                if (Time.time - lastAttackTime > data.attackRate)
                {
                    if (FenceAttacked != null)
                    {
                        lastAttackTime = Time.time;
                        FenceAttacked.TakeDamage(data.basicATK);
                        animator.speed = 1;
                        animator.SetTrigger("Attack");
                    }
                }
            }
        }
        else
        {
            SetState(AIState.Attacking);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Fence"))
        {
            SetState(AIState.AttackingFence);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Fence"))
        {
            SetState(AIState.Attacking);
        }
    }

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
            case AIState.Die:
                agent.speed = 0;
                agent.isStopped = true;
                animator.SetTrigger("Dying");
                Invoke("Die", 4.0f);
                break;

        }

        animator.SetFloat("Speed", agent.speed);
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Debug.Log("목표 탐색 시작");
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

    private bool IsFenceInFront()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        RaycastHit hit;

        //Ray로할지, Tag로할지는 미정
        if (Physics.Raycast(ray, out hit, data.attackDistance, fenceMaskLayer))
        {
            FenceAttacked = hit.collider.GetComponent<IDamagable>();
            return true;
        }
        else return false;
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < data.fieldOfView * 0.5f;
    }

    public void TakeDamage(float damage)
    {
        data.maxHealth -= damage;

        if (data.maxHealth <= 0)
        {
            SetState(AIState.Die);
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
        //아이템 드롭부분
        for (int i = 0; i < data.dropOnDeath.Length; i++)
        {
            Debug.Log($"{data.dropOnDeath[i].dropPrefab}");
            //Instantiate(data.dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
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

    //private void OnDrawGizmos()
    //{
    //    // Gizmo 색상을 설정
    //    Gizmos.color = Color.red;

    //    // 공격 범위를 원형으로 표시
    //    Gizmos.DrawWireSphere(transform.position, data.attackDistance);

    //    // 씬 뷰에 거리 값을 표시
    //    Vector3 textPosition = (transform.position + CharacterManager.Instance.player.transform.position) / 2;
    //    UnityEditor.Handles.Label(textPosition, $"Distance: {playerDistance:F2}");
    //}

    public void StopZombie()
    {
        isStopped = true;
    }

    public void ResumeZombie()
    {
        isStopped = false;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
