using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLevelManager : Singleton<WorldLevelManager>
{
    [Header("Drop Item")]
    public Transform ItemObjectsParent;

    [Header("Building System")]
    public Transform buildingObjectsParent;
    [HideInInspector] public BuildingSystem buildingSystem;

    private void Start()
    {
        buildingSystem = GetComponent<BuildingSystem>();
    }
}
