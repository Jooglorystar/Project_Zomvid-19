using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    protected ItemObject itemObject;

    public virtual void Start()
    {
        itemObject = GetComponent<ItemObject>();
    }
    public virtual void OnAttackInput()
    {

    }

    public virtual void OnRunInput(bool isRunning)
    {

    }
}
