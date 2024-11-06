using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] List<Collider> colliders = new();
    [SerializeField] Material green;
    [SerializeField] Material red;
    [HideInInspector] public BuildObjectSO.BuildType buildType;
    [HideInInspector] public bool isBuildable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 14 && buildType == BuildObjectSO.BuildType.Foundation)
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 14 && buildType == BuildObjectSO.BuildType.Foundation)
        {
            colliders.Remove(other);
        }
    }

    private void Update()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (colliders.Count == 0)
        {
            isBuildable = true;
        }
        else
        {
            isBuildable = false;
        }
        
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                materials[i] = isBuildable ? green : red;
            }
            renderer.materials = materials;
        }
    }
}
