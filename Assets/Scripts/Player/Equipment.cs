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

    public void newEquip(ItemSO equip)
    {
        if (curEquip == equip) return;
        else if (curEquip != null)
        {
            unEquip();

        }
        else
        {
            if (equip is MeleeEquipItemSO meleeEquipItem)
            {
                curEquip = Instantiate(meleeEquipItem.meleePrefab, HandPos).GetComponent<Equip>();
            }
            else if (equip is RangeEquipItemSO rangeEquipItem)
            {
                curEquip = Instantiate(rangeEquipItem.rangePrefab, HandPos).GetComponent<Equip>();
            }
        }
        Debug.Log(curEquip);
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
