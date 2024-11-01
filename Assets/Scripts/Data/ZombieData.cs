using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieData : EntityData
{
    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public float attackRate;
    public float attackDistance;

    [Header("Detect")]
    public float detectDistance;
    public float fieldOfView = 120f;

    //public ItemData[] dropOnDeath;
}
