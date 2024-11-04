using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class nmsObject : MonoBehaviour
{
    private NavMeshSurface nms;

    private void Awake()
    {
        nms = GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        if (IsFarFromPlayer())
        {
            nms.BuildNavMesh();
        }
    }

    private bool IsFarFromPlayer()
    {
        if(Vector3.Distance(transform.position, CharacterManager.Instance.player.transform.position) > 30)
        {
            transform.position = CharacterManager.Instance.player.transform.position;
            return true;
        }
        return false;
    }
}
