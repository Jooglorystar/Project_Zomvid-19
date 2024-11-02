using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    private bool isAttacking;

    private float damage;
    private float attackDistance;
    private float attackRate;
    private float useStamina;

    [SerializeField] private ItemSO itemData;

    private Animator animator;
    private Camera cam;


    private void Start()
    {
        InitMelee();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    public void InitMelee()
    {
        if (itemData is MeleeEquipItemSO meleeData)
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
            }
        }
    }

    public override void OnRunInput(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }



    private void OnCanAttack()
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
        }
    }
}
