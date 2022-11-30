using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePooler : MonoBehaviour
{
    public static EnemyProjectilePooler enemyProjectilePool;

    private List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPool = 3;
    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        if (enemyProjectilePool == null)
        {
            enemyProjectilePool = this;
        }
    }

    private void addProjectileToPool()
    {
        GameObject obj = Instantiate(projectilePrefab, enemyProjectilePool.transform);
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
