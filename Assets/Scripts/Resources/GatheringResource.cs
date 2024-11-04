using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatheringResource : MonoBehaviour, IInteractable, IDamagable
{
    [Serializable]
    private class ItemStack
    {
        public ItemSO itemSO;
        public int stack;
    }

    [SerializeField] private string resourceName;
    [SerializeField] private float durability; // 최대 체력
    private float curDurability;

    [SerializeField] private List<ToolType> availableTools;      // 파밍 가능 장비 타입
    [SerializeField] private string requiredToolDescription;    // 파밍 가능 장비 설명
    [SerializeField] private string gatherDescription;          // 파밍 키 설명

    [SerializeField] private List<ItemStack> ListEarlyGatherings; // 체력이 깎일 때마다 비율로 반환
    [SerializeField] private List<ItemStack> ListEndGatherings;   // 체력이 전부 깎였을 때 한번에 반환

    [SerializeField] private Transform normalModel;
    [SerializeField] private Transform depletedModel;

    [SerializeField] private bool regenerative;    //재생 가능 여부
    [SerializeField] private float regenerateTime; //재생 시간 (단위 : 하루)
    private bool depleted;
    private float depletedTime;


    private void Start()
    {
        curDurability = durability;
    }

    public void OnInteraction() // 없음
    {
    }

    public string GetInteractPromptName()
    {
        return resourceName;
    }

    public string GetInteractPromptDescription()
    {
        if (CanGather())
        {
            return gatherDescription;
        }
        else
        {
            return requiredToolDescription;
        }
    }


    public void TakeDamage(float damage)
    {
        if (depleted == true || CanGather() == false) return;

        float after = Mathf.Max(curDurability - damage, 0);
        Dictionary<ItemIdentifier, int> gatherItems = new();

        foreach (ItemStack item in ListEarlyGatherings)
        {
            int beforeRemain = (int)(curDurability / durability * item.stack);
            int afterRemain = (int)(after / durability * item.stack);
            int gather = beforeRemain - afterRemain;
            if (gather >= 0)
            {
                gatherItems.Add(item.itemSO.identifier, gather);
            }
        }
        if (gatherItems.Count > 0)
        {
            // TODO : 인벤토리 호출 -> 아이템 소매넣기
        }
        curDurability = after;

        if (curDurability <= 0)
        {
            Depleted();
        }
    }

    

    private bool CanGather()
    {
        if (CharacterManager.Instance.player.equip.curEquip is EquipTool equipTool)
        {
            //if (equipTool.itemData is MeleeEquipItemSO meleeEquip)
            //{
            //    if (availableTools.Contains(meleeEquip.toolType))
            //    {
            //        return true;
            //    }
            //}
        }

        return false;
    }

    private void Depleted()
    {
        // TODO : ListEndGatherings 전부 게임오브젝트로

        normalModel.gameObject.SetActive(false);
        depletedModel.gameObject.SetActive(true);

        depleted = true;
        depletedTime = EnvironmentManager.Instance.WorldTime;

        // 재생 가능하면 설정된 시간 뒤에 재생됨
        if (regenerative)
        {
            EnvironmentManager.Instance.SetAlarm(depletedTime + regenerateTime, Regenerated);
        }
    }

    private void Regenerated()
    {
        curDurability = durability;

        normalModel.gameObject.SetActive(true);
        depletedModel.gameObject.SetActive(false);

        depleted = false;
    }
}
