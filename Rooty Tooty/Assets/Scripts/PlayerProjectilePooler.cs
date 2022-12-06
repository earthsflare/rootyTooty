using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectilePooler : MonoBehaviour
{
    public static PlayerProjectilePooler playerProjectilePool;
    // All player projectiles start with 3 in pooler
    // May wish to have different amounts for each projectile
    // More complicated, probably out of scope
    [SerializeField] private int amountToPool = 3;

    [Header("Prefab Reference")]
    [SerializeField] private GameObject[] projectilesPrefab;

    [Header("Debug: Read Only")]
    [SerializeField] private List<List<GameObject>> pooledProjectiles = new List<List<GameObject>>();

    private void Awake()
    {
        if (playerProjectilePool == null)
        {
            playerProjectilePool = this;
        }
    }

    private void addProjectileToPool(int projectileType)
    {
        GameObject obj = Instantiate(projectilesPrefab[projectileType]);
        obj.transform.parent = transform;
        obj.SetActive(false);
        pooledProjectiles[projectileType].Add(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < projectilesPrefab.Length; i++)
        {
            pooledProjectiles.Add(new List<GameObject>());
            while(pooledProjectiles[i].Count < amountToPool)
            {
                addProjectileToPool(i);
            }
        }
    }

    void Update()
    {

    }

    public GameObject GetPooledObject(int projectileType)
    {
        if (projectileType < pooledProjectiles.Count)
        {
            int poolCount = pooledProjectiles[projectileType].Count;
            for (int i = 0; i < poolCount; i++)
            {
                // Return projectile if inactive
                if (!pooledProjectiles[projectileType][i].activeInHierarchy)
                {
                    return pooledProjectiles[projectileType][i];
                }

                // Add more projectiles if pool is empty and projectile is requested
                if (i == poolCount - 1)
                {
                    addProjectileToPool(projectileType);
                    if (!pooledProjectiles[projectileType][i+1].activeInHierarchy)
                    {
                        return pooledProjectiles[projectileType][i+1];
                    }
                }
            }
        }
        return null;
    }
    
    public int getProjectilePrefabCount()
    {
        return projectilesPrefab.Length;
    }
        
    public string getProjectileName(int projectileType)
    {
        if (projectileType < projectilesPrefab.Length)
        {
            return projectilesPrefab[projectileType].name;
        }
        return "Invalid projectile type";
    }

}
