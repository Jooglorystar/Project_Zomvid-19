using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInstanceToObject : MonoBehaviour
{
    public Terrain terrain;               // ������ ��ġ�� Terrain
    public int prototypeIndex;              // Tree Layer Index
    public GameObject treePrefabs;         // ��ȣ�ۿ� ������ ���� ������
    public Transform parent;

    void Start()
    {
        ConvertTreesToGameObjects();
    }

    void ConvertTreesToGameObjects()
    {
        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] trees = terrainData.treeInstances;
        List<TreeInstance> treesRemain = new List<TreeInstance>();
        treesRemain.Capacity = trees.Length;

        foreach (TreeInstance tree in trees)
        {
            if (tree.prototypeIndex == prototypeIndex)
            {
                // Terrain ��ǥ�� ���� ��ǥ�� ��ȯ
                Vector3 worldPosition = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;

                // ������ ũ��� ȸ�� ����
                GameObject treeObject = Instantiate(treePrefabs, worldPosition, Quaternion.identity);
                treeObject.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);

                // ȸ�� ����
                float rotationAngle = tree.rotation * Mathf.Rad2Deg;
                treeObject.transform.Rotate(0, rotationAngle, 0);

                // �θ� ����
                treeObject.transform.parent = parent;
            }
            else
            {
                treesRemain.Add(tree);
            }
        }

        // Terrain���� ������ȭ �� ���� ����
        terrainData.treeInstances = treesRemain.ToArray();
    }
}
