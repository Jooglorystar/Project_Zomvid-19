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
        //인벤토리에서 모든 빈 물통을 채워진 물통으로 변경. 인벤토리에서 만드는게 좋아보임.
        CharacterManager.Instance.player.uiInventoryTab.FillWithLiquid(Water_Full);
    }
}
