using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    // Transform for the origin of the enemy projectile
    public Transform firePoint;
    // Enemy's projectile
    public GameObject projectileToFire;
    // Used to make sure enemy can't shoot while weapon on cooldown
    public bool isAvailable = true;
    // Weapon cooldown
    public float cooldownDuration = 2.0f;

    // Update is called once per frame
    void Update()
    {
        // Get projectile from the projectile pool
        GameObject projectile = EnemyProjectilePooler.enemyProjectilePool.GetPooledObject();

        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;
            projectile.SetActive(true);
        }
        StartCoroutine(StartCooldown());
    }

    // Coroutine for cooldown on enemy weapon
    private IEnumerator StartCooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(cooldownDuration);
        isAvailable = true;
    }
}
