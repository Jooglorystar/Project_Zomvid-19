using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    [HideInInspector]
    public ItemObject itemData;

    public virtual void Start()
    {
        itemData = GetComponent<ItemObject>();
    }
    public virtual void OnAttackInput()
    {

    }
    public virtual void OnRunInput(bool isRunning)
    {

    }
}
