using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private Terrain terrain;

    [SerializeField] private List<BuildObjectSO> buildObjects = new();
    private BuildObjectSO currentObject;
    private PreviewObject currentPreview;
    private Vector3 currentPos;

    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask buildPosTargetLayer;

    [SerializeField] private float rayDistance = 5.0f;
    [SerializeField] private float offset = 1.0f;
    [SerializeField] private Vector3 gridSize = new Vector3(2.0f, 0.5f, 2.0f);

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
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, rayDistance, buildPosTargetLayer))
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
        Vector3 NormalizedVector = hit.point;

        if (currentObject.buildType != BuildObjectSO.BuildType.Ground)
        {
            NormalizedVector -= Vector3.one * offset;
            NormalizedVector.Scale(new Vector3(1 / gridSize.x, 1 / gridSize.y, 1 / gridSize.z));
            NormalizedVector = new Vector3(Mathf.Round(NormalizedVector.x), Mathf.Round(NormalizedVector.y), Mathf.Round(NormalizedVector.z));
            NormalizedVector.Scale(gridSize);
            NormalizedVector += Vector3.one * offset;
        }

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) // �� ���̾ ��Ҵ��� Ȯ��
        {
            float groundHeight = terrain.SampleHeight(currentPos) + terrain.transform.position.y;
            while (NormalizedVector.y < groundHeight) // ���� ���̺��� ������
            {
                NormalizedVector.y += gridSize.y;     // ��ġ�� ���� ���̷� �ø�
            }
        }

        currentPos = NormalizedVector;
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
