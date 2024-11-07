using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipTool : Equip
{
    private bool isAttacking;

    private float damage;
    private float attackDistance;
    private float attackRate;
    private float useStamina;

    private Animator animator;
    private Camera cam;

    [SerializeField] private AudioClip swingClip;
    [SerializeField] private AudioClip attackClip;
    private AudioSource audioSource;

    public override void Start()
    {
        base.Start();
        InitMelee();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    public void InitMelee()
    {
        if (itemData.itemStack.itemSO is MeleeEquipItemSO meleeData)
        {
            damage = meleeData.damage;
            attackDistance = meleeData.attackDistance;
            attackRate = meleeData.attackRate;
            useStamina = meleeData.useStamina;
        }
    }

    public override void OnAttackInput()
    {
        if (!isAttacking)
        {
            if (CharacterManager.Instance.player.condition.UseStamina(useStamina))
            {
                isAttacking = true;
                animator.SetTrigger("OnAttack");
                Invoke("OnCanAttack", attackRate);
                if(swingClip != null)
                    audioSource.PlayOneShot(swingClip);
            }
        }
    }

    public override void OnRunInput(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }



    public void OnCanAttack()
    {
        isAttacking = false;
    }


    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (hit.collider.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                Debug.Log("OnHit!");
                damagable.TakeDamage(damage);
            }
            else if (hit.collider.transform.parent != null)
            {
                if (hit.collider.transform.parent.TryGetComponent<IDamagable>(out IDamagable damagable2))
                {
                    Debug.Log("OnHit!");
                    damagable2.TakeDamage(damage);
                    if(attackClip != null)
                        audioSource.PlayOneShot(attackClip);
                }
            }
        }
    }
}
