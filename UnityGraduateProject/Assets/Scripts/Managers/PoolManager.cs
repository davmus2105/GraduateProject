using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    Transform poolfolder;

    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        try
        {
            poolfolder = GameObject.Find("[POOL]").transform;
        }
        catch
        {
            poolfolder = new GameObject("[POOL]").transform;
        }                    
        FillPoolOnStart();        
    }

    void FillPoolOnStart()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        if (pools == null || pools.Count == 0)
            return;
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab, poolfolder);
                go.SetActive(false);
                objPool.Enqueue(go);
            }
            poolDictionary.Add(pool.poolname, objPool);
        }
    }

    public GameObject Spawn(string poolname, Vector3 position, Quaternion rotation, GameObject go = null)
    {
        GameObject spawngo = null;
        if (!poolDictionary.ContainsKey(poolname))
        {
            if (go == null)
            {
                Debug.Log("Pool with name " + poolname + " doesn't exist. And it can't be instantiated");
                return spawngo;
            }
            Debug.Log("Pool with name " + poolname + " doesn't exist. So it was instantiated");
            spawngo = Instantiate(go, position, rotation);
            return spawngo;
        }
        spawngo = poolDictionary[poolname].Dequeue();
        spawngo.SetActive(true);
        spawngo.transform.position = position;
        spawngo.transform.rotation = rotation;
        return spawngo;
    }
    public GameObject Spawn(string poolname, GameObject go = null, Transform parent = null)
    {
        GameObject spawngo = null;
        if (!poolDictionary.ContainsKey(poolname))
        {
            if (go == null)
            {
                Debug.Log("Pool with name " + poolname + " doesn't exist. And it can't be instantiated");
                return spawngo;
            }
            Debug.Log("Pool with name " + go.name + " doesn't exist. So it was instantiated");
            if (!parent)
                spawngo = Instantiate(go, Vector3.zero, new Quaternion(0,0,0,0), parent);
            else
                spawngo = Instantiate(go, Vector3.zero, new Quaternion(0, 0, 0, 0));
            return spawngo;
        }
        spawngo = poolDictionary[go.name].Dequeue();
        spawngo.transform.SetParent(parent);
        spawngo.SetActive(true);
        spawngo.transform.position = Vector3.zero;
        spawngo.transform.rotation = new Quaternion(0, 0, 0, 0);
        return spawngo;
    }


    public void PoolObj(string poolname, GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(poolfolder);
        if (!poolDictionary.ContainsKey(poolname))
        {
            Queue<GameObject> queuepool = new Queue<GameObject>();
            queuepool.Enqueue(go);
            poolDictionary.Add(poolname, queuepool);
            return;
        }
        poolDictionary[poolname].Enqueue(go);
    }


    [System.Serializable]
    public class Pool
    {
        public string poolname;
        public GameObject prefab;
        public int size;
    }
}
