using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInstanceToObject : MonoBehaviour
{
    public Terrain terrain;               // ������ ��ġ�� Terrain
    public GameObject[] treePrefabs;         // ��ȣ�ۿ� ������ ���� ������

    void Start()
    {
        ConvertTreesToGameObjects();
    }

    void ConvertTreesToGameObjects()
    {
        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] trees = terrainData.treeInstances;

        foreach (TreeInstance tree in trees)
        {
            // Terrain ��ǥ�� ���� ��ǥ�� ��ȯ
            Vector3 worldPosition = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;

            // ������ ũ��� ȸ�� ����
            GameObject treeObject = Instantiate(treePrefabs[tree.prototypeIndex], worldPosition, Quaternion.identity);
            treeObject.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);

            // ȸ�� ����
            float rotationAngle = tree.rotation * Mathf.Rad2Deg;
            treeObject.transform.Rotate(0, rotationAngle, 0);
        }

        // Terrain���� ���� ����
        terrainData.treeInstances = new TreeInstance[0];
    }
}
