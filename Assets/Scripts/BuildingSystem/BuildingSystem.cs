using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private List<BuildObjectSO> buildObjects = new();
    private BuildObjectSO currentObject;
    private PreviewObject currentPreview;
    private Vector3 currentPos;

    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask buildPosTargetLayer;

    [SerializeField] private float offset = 1.0f;
    [SerializeField] private float gridSize = 1.0f;


    private void Update()
    {
        if (CharacterManager.Instance.player.controller.isBuilding)
        {
            UpdatePreview();
        }
    }

    public bool LoadBuildObject(ItemIdentifier identifier)
    {
        currentObject = buildObjects.Find(i => i.identifier == identifier);
        if (currentObject == null)
        {
            return false;
        }

        currentPreview = Instantiate(currentObject.Preview, currentPos, Quaternion.identity);
        currentPreview.buildType = currentObject.buildType;
        return true;
    }

    private void UpdatePreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 10, buildPosTargetLayer))
        {
            currentPreview.gameObject.SetActive(true);
            if (hit.transform != transform)
            {
                NormalizePosition(hit);
            }
        }
        else
        {
            currentPreview.gameObject.SetActive(false);
        }
    }

    private void NormalizePosition(RaycastHit hit)
    {
        currentPos = hit.point;
        currentPos -= Vector3.one * offset;
        currentPos /= gridSize;
        currentPos = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
        currentPos *= gridSize;
        currentPos += Vector3.one * offset;
        currentPreview.transform.position = currentPos;
    }

    public bool OnBuild()
    {
        if (currentPreview.gameObject.activeSelf && currentPreview.isBuildable)
        {
            Instantiate(currentObject.Building, currentPos, Quaternion.identity);
            return true;
        }

        return false;
    }

    public void ExitBuild()
    {
        Destroy(currentPreview.gameObject);
    }
}
