using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectilePooler : MonoBehaviour
{
    public static PlayerProjectilePooler playerProjectilePool;

    private List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPool = 3;
    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        if (playerProjectilePool == null)
        {
            playerProjectilePool = this;
        }
    }

    private void addProjectileToPool()
    {
        GameObject obj = Instantiate(projectilePrefab);
        obj.SetActive(false);
        pooledProjectiles.Add(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            addProjectileToPool();
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledProjectiles[i].activeInHierarchy)
            {
                // pooledProjectiles[i].transform.SetParent(transform, true);
                return pooledProjectiles[i];
            }
            if (i == amountToPool - 1)
            {
                addProjectileToPool();
                amountToPool++;
            }
        }
        return null;
    }
}