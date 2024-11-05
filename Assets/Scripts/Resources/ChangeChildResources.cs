using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChildResources : MonoBehaviour
{
    public Transform oldParentObject; // �ش� ������Ʈ�� �ڽ� ������Ʈ�� ���� ����
    public Transform newParentObject;
    public GameObject newPrefab;         // ��ȣ�ۿ� ������

    void Start()
    {
        ConvertChildObjects();
    }

    void ConvertChildObjects()
    {
        foreach (Transform chid in oldParentObject)
        {
            // ���� ������Ʈ�� ��ġ, ȸ��, ũ�� �б�
            Vector3 oldPosition = chid.position;
            Quaternion oldRotation = chid.rotation;
            Vector3 oldScale = chid.localScale;

            // ���ο� ������ �ν��Ͻ� ����
            GameObject newClone = Instantiate(newPrefab, oldPosition, oldRotation);
            newClone.transform.localScale = oldScale; // ���� ũ�� ����
            newClone.transform.parent = newParentObject; // �θ� ������Ʈ�� ����

            // ���� Ŭ�� ������Ʈ �ı�
            Destroy(chid.gameObject);
        }
    }
}
