using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            curEquip?.OnAttackInput();
        }
    }
}
