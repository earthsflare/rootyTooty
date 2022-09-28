using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooler : MonoBehaviour
{
    public static ProjectilePooler playerProjectilePool;

    private List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPool = 20;
    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        if (playerProjectilePool == null)
        {
            playerProjectilePool = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pooledProjectiles.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if(!pooledProjectiles[i].activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }
        return null;
    }
}
