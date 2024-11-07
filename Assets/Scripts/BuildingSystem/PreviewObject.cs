using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] Material green;
    [SerializeField] Material red;
    [SerializeField] LayerMask ignoreColliderLayers;
    [HideInInspector] public BuildObjectSO.BuildType buildType;
    [HideInInspector] public bool isBuildable;

    private int exclusiveColliders;
    private int necessaryColliders;

    private void OnDisable()
    {
        exclusiveColliders = 0;
        necessaryColliders = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsLayerMatched(ignoreColliderLayers, other.gameObject.layer))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            exclusiveColliders++;
            return;
        }

        var colliderBuildObject = other.GetComponent<Constructure>();

        switch (buildType)
        {
            case BuildObjectSO.BuildType.Foundation:
                if (colliderBuildObject != null)
                {
                    if (colliderBuildObject.buildType == BuildObjectSO.BuildType.Foundation)
                    {
                        exclusiveColliders++;
                    }
                }
                break;
            case BuildObjectSO.BuildType.Wall:
                if (colliderBuildObject != null)
                {
                    if (colliderBuildObject.buildType == BuildObjectSO.BuildType.Foundation)
                    {
                        necessaryColliders++;
                    }
                    else
                    {
                        exclusiveColliders++;
                    }
                }
                else
                {
                    exclusiveColliders++;
                }
                break;
            case BuildObjectSO.BuildType.Ground:
                exclusiveColliders++;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsLayerMatched(ignoreColliderLayers, other.gameObject.layer))
        {
            return;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            exclusiveColliders--;
            return;
        }

        var colliderBuildObject = other.GetComponent<Constructure>();

        switch (buildType)
        {
            case BuildObjectSO.BuildType.Foundation:
                if (colliderBuildObject != null)
                {
                    if (colliderBuildObject.buildType == BuildObjectSO.BuildType.Foundation)
                    {
                        exclusiveColliders--;
                    }
                }
                break;
            case BuildObjectSO.BuildType.Wall:
                if (colliderBuildObject != null)
                {
                    if (colliderBuildObject.buildType == BuildObjectSO.BuildType.Foundation)
                    {
                        necessaryColliders--;
                    }
                    else
                    {
                        exclusiveColliders--;
                    }
                }
                else
                {
                    exclusiveColliders--;
                }
                break;
            case BuildObjectSO.BuildType.Ground:
                exclusiveColliders--;
                break;
        }
    }

    private void Update()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        switch (buildType)
        {
            case BuildObjectSO.BuildType.Foundation:
            case BuildObjectSO.BuildType.Ground:
                isBuildable = exclusiveColliders == 0;
                break;
            case BuildObjectSO.BuildType.Wall:
                isBuildable = necessaryColliders > 0 && exclusiveColliders == 0;
                break;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                materials[j] = isBuildable ? green : red;
            }
            renderers[i].materials = materials;
        }
    }

    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }
}

