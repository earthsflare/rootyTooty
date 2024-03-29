using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HealthPickupPool : MonoBehaviour
{
    public static HealthPickupPool instance { get; private set; }

    public List<GameObject> pooledObjects;
    [SerializeField] public GameObject objectToPool;
    public int amountToPool = 10;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i < amountToPool; i++)
        {
            addPooledObject();
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return addPooledObject();
    }

    private GameObject addPooledObject()
    {
        GameObject tmp;
        tmp = Instantiate(objectToPool, gameObject.transform);
        tmp.SetActive(false);
        pooledObjects.Add(tmp);
        return tmp;
    }
}