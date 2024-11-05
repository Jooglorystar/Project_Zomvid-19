using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChildResources : MonoBehaviour
{
    public Transform oldParentObject; // 해당 오브젝트의 자식 오브젝트를 전부 변경
    public Transform newParentObject;
    public GameObject newPrefab;         // 상호작용 프리팹

    void Start()
    {
        ConvertChildObjects();
    }

    void ConvertChildObjects()
    {
        foreach (Transform chid in oldParentObject)
        {
            // 기존 오브젝트의 위치, 회전, 크기 읽기
            Vector3 oldPosition = chid.position;
            Quaternion oldRotation = chid.rotation;
            Vector3 oldScale = chid.localScale;

            // 새로운 프리팹 인스턴스 생성
            GameObject newClone = Instantiate(newPrefab, oldPosition, oldRotation);
            newClone.transform.localScale = oldScale; // 기존 크기 적용
            newClone.transform.parent = newParentObject; // 부모 오브젝트에 연결

            // 기존 클론 오브젝트 파괴
            Destroy(chid.gameObject);
        }
    }
}
