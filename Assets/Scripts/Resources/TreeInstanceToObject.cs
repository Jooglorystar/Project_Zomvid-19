using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInstanceToObject : MonoBehaviour
{
    public Terrain terrain;               // 나무가 배치된 Terrain
    public int prototypeIndex;              // Tree Layer Index
    public GameObject[] treePrefabs;         // 상호작용 가능한 나무 프리팹

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
                // Terrain 좌표를 월드 좌표로 변환
                Vector3 worldPosition = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;

                // 나무의 크기와 회전 설정
                GameObject treeObject = Instantiate(treePrefabs[tree.prototypeIndex], worldPosition, Quaternion.identity);
                treeObject.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);

                // 회전 적용
                float rotationAngle = tree.rotation * Mathf.Rad2Deg;
                treeObject.transform.Rotate(0, rotationAngle, 0);
            }
            else
            {
                treesRemain.Add(tree);
            }
        }

        // Terrain에서 프리팹화 한 나무 제거
        terrainData.treeInstances = treesRemain.ToArray();
    }
}
