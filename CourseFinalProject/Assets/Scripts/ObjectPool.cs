using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for (int i = 0; i < pools.Count; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int j = 0; j < pools[i].size; j++)
            {
                objectPool.Enqueue(CreateObject(i));
            }

            poolDictionary.Add(pools[i].tag, objectPool);
        }
    }

    public GameObject CreateObject(int i)
    {
        GameObject obj = Instantiate(pools[i].prefab);
        obj.SetActive(false);
        obj.transform.parent = transform;
        return obj;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Poll with tag " + tag + " does not exist!");
            return null;
        }

        GameObject objectToSpawn = null;

        if (poolDictionary[tag].Count == 0)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (string.Equals(pools[i].tag, tag))
                {
                    objectToSpawn = CreateObject(i);
                    objectToSpawn.SetActive(true);
                    objectToSpawn.transform.position = position;
                    objectToSpawn.transform.rotation = rotation;

                    objectToSpawn.GetComponent<IPooledObject>().SetObjectPool(this);
                    break;
                }
            }
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.GetComponent<IPooledObject>().SetObjectPool(this);
        }

        return objectToSpawn;
    }
}
