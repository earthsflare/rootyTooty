using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    // Transform for the origin of the player projectile
    public Transform firePoint;

    // Used to make sure player can't shoot while weapon on cooldown
    public bool isAvailable = true;
    // Used to make sure player can't shoot fireballs and waterballs while on cooldown
    public bool canSwitchWeapon = true;
    // Weapon cooldown
    public float cooldownDuration = 1.0f;
    // Magic weapon cooldown
    public float magicCooldownDuration = 5.0f;
    // Player's current projectile
    public int currentProjectile = 0;

    private bool fireballEnabled = true;

    public void ToggleFireball(bool enabled) { fireballEnabled = enabled; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("NextProjectile") && canSwitchWeapon && fireballEnabled)
        {
            currentProjectile += 1;
            if (currentProjectile == PlayerProjectilePooler.playerProjectilePool.getProjectilePrefabCount())
            {
                currentProjectile = 0;
            }
            Debug.Log("Changed projectile to : " + PlayerProjectilePooler.playerProjectilePool.getProjectileName(currentProjectile));
        }

        magicCooldownDuration -= Time.deltaTime;
        if (magicCooldownDuration <= 0f)
        {
            canSwitchWeapon = true;
        }

        if (Input.GetButton("Fire1") && isAvailable && Time.timeScale != 0f)
        {
            // Get projectile from the projectile pool
            GameObject projectile = PlayerProjectilePooler.playerProjectilePool.GetPooledObject(
                currentProjectile);

            if (projectile != null)
            {
                // Code to calculate projectile trajectory
                Vector2 mouse = Input.mousePosition;
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
                Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
                float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

                // Aims projectile transform at the position of the mouse
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
                projectile.SetActive(true);

                if (currentProjectile > 0)
                {
                    currentProjectile = 0;
                    magicCooldownDuration = 5.0f;
                    canSwitchWeapon = false;
                }
                StartCoroutine(StartCooldown());
            }
        }
    }

    // Coroutine for cooldown on player weapon
    private IEnumerator StartCooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(cooldownDuration);
        isAvailable = true;
    }
}
