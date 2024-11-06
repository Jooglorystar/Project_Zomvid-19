using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Constructure : MonoBehaviour, IDamagable
{
    public BuildObjectSO.BuildType buildType;
    [SerializeField] private float durability;
    [SerializeField] private List<ItemStack> BreakResources;   // 체력이 전부 깎였을 때 반환

    private float curDurability;

    private void Start()
    {
        curDurability = durability;
    }

    public void TakeDamage(float damage)
    {
        float after = Mathf.Max(curDurability - damage, 0);

        curDurability = after;
        if (curDurability <= 0)
        {
            Breaked();
        }
    }

    private void Breaked()
    {
        foreach (ItemStack item in BreakResources)
        {
            GameObject drops = Instantiate(item.itemSO.dropPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            drops.transform.parent = WorldLevelManager.Instance.ItemObjectsParent;
            var itemObject = drops.GetComponent<ItemObject>();
            if (itemObject != null)
            {
                itemObject.itemStack.stack = item.stack;
            }
        }

        Destroy(gameObject);
    }
}
