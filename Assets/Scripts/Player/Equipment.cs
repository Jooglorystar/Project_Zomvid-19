using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform HandPos;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            curEquip?.OnAttackInput();
        }
    }

    public void newEquip(MeleeEquipItemSO equip)
    {
        if (curEquip == equip) return;
        else if (curEquip != null)
        {
            unEquip();
        }
        curEquip = Instantiate(equip.meleePrefab, HandPos).GetComponent<Equip>();
        curEquip.gameObject.SetActive(true);
    }

    public void unEquip()
    {
        if (curEquip == null)
            return;
        else
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
}
