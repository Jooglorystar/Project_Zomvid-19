using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : MonoBehaviour, IInteractable
{
    [SerializeField] string riverName;
    [SerializeField] string riverDescription;
    [SerializeField] ItemSO Water_Full;

    public string GetInteractPromptDescription()
    {
        return riverDescription;
    }

    public string GetInteractPromptName()
    {
        return riverName;
    }

    public void OnInteraction()
    {
        //�κ��丮���� ��� �� ������ ä���� �������� ����. �κ��丮���� ����°� ���ƺ���.
        CharacterManager.Instance.player.uiInventoryTab.FillWithLiquid(Water_Full);
    }
}
