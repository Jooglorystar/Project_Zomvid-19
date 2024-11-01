using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PoolObject
{
    bullet
}

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public int size;
        public GameObject prefab;
        public PoolObject tag;  //지금은 총알 하나만 사용해서 임시로, 나중에 전체적으로 필요할 때에 변경가능
    }

    public List<Pool> pools;
    private Dictionary<PoolObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        GameManager.Instance.objectPool = this;
        Init();
    }

    private void Init()
    {
        poolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();
        foreach(var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetFromPool(PoolObject tag)
    {
        if (poolDictionary[tag].All(x => x.activeSelf == true))
        {
            Pool pool = pools.Find(x => x.tag == tag);
            GameObject newObj = Instantiate(pool.prefab, this.transform);
            newObj.SetActive(false);
            poolDictionary[tag].Enqueue(newObj);
            return newObj;
        }
        else if (poolDictionary[tag].TryDequeue(out GameObject obj))
        {
            poolDictionary[tag].Enqueue(obj);
            return obj;
        }
        return null;
    }
}