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
    Die
}

public abstract class LivingEntity<T> : MonoBehaviour where T : EntityData, IDamagable
{
    protected T data;

    [Header("AI")]
    private NavMeshAgent agent;
    private AIState aiState;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private Coroutine coroutine;

    private void Awake()
    {
        data = GetComponent<T>();
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

    public virtual void TakeDamage(float damage)
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
}
