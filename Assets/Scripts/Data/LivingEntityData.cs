using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityData : EntityData
{
    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Detect")]
    public float detectDistance;
    public float fieldOfView = 120f;

    public ItemSO[] dropOnDeath;
}
