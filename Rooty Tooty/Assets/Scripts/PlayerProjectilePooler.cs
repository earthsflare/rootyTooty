using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerProjectileType
{
    Firebolt,
    Waterball,
    MaxNumPlayerProjectiles
}
public class PlayerProjectilePooler : MonoBehaviour
{
    public static PlayerProjectilePooler playerProjectilePool;
    // All player projectiles start with 3 in pooler
    // May wish to have different amounts for each projectile
    // More complicated, probably out of scope
    [SerializeField] private int amountToPool = 3;
    [SerializeField] public int currentPlayerProjectile = 0;

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

    private void addProjectileToPool(PlayerProjectileType projectileType)
    {
        GameObject obj = Instantiate(projectilesPrefab[(int)projectileType]);
        obj.transform.parent = transform;
        obj.SetActive(false);
        pooledProjectiles[(int)projectileType].Add(obj);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < (int)PlayerProjectileType.MaxNumPlayerProjectiles; i++)
        {
            pooledProjectiles.Add(new List<GameObject>());
            while(pooledProjectiles[i].Count < amountToPool)
            {
                addProjectileToPool((PlayerProjectileType)i);
            }
        }
    }

    void Update()
    {
        if (Input.GetButton("NextProjectile"))
        {
            currentPlayerProjectile += 1;
            if (currentPlayerProjectile == (int)PlayerProjectileType.MaxNumPlayerProjectiles)
            {
                currentPlayerProjectile = 0;
            }
        }
    }

    public GameObject GetPooledObject(PlayerProjectileType projectileType)
    {
        Debug.Log("count " + pooledProjectiles.Count);
        int poolCount = pooledProjectiles[(int)projectileType].Count;
        for (int i = 0; i < poolCount; i++)
        {
            // Return projectile if inactive
            if (!pooledProjectiles[(int)projectileType][i].activeInHierarchy)
            {
                return pooledProjectiles[(int)projectileType][i];
            }

            // Add more projectiles if pool is empty and projectile is requested
            if (i == poolCount - 1)
            {
                addProjectileToPool(projectileType);
                if (!pooledProjectiles[(int)projectileType][i+1].activeInHierarchy)
                {
                    return pooledProjectiles[(int)projectileType][i+1];
                }
            }
        }
        return null;
    }

    public int getCurrentPlayerProjectileType()
    {
        return currentPlayerProjectile;
    }
}
