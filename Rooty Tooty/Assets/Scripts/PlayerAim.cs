using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    // Main Camera for mouse aiming calculations
    private Camera mainCam;
    // Transform for the origin of the player projectile
    public Transform firePoint;

    // Used to make sure player can't shoot while weapon on cooldown
    public bool isAvailable = true;
    // Weapon cooldown
    public float cooldownDuration = 01.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f)
        {
            isAvailable = false;
        }
        else
        {
            if (Input.GetMouseButton(0) && isAvailable)
            {
                // Get projectile from the projectile pool
                GameObject projectile = PlayerProjectilePooler.playerProjectilePool.GetPooledObject(
                    (PlayerProjectileType)PlayerProjectilePooler.playerProjectilePool.getCurrentPlayerProjectileType());

                if (projectile != null)
                {
                    // Code to calculate projectile trajectory
                    Vector2 mouse = Input.mousePosition;
                    Vector2 screenPoint = mainCam.WorldToScreenPoint(transform.localPosition);
                    Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
                    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                    firePoint.rotation = Quaternion.Euler(0f, 0f, angle);

                    // Aims projectile transform at the position of the mouse
                    projectile.transform.position = firePoint.position;
                    projectile.transform.rotation = firePoint.rotation;
                    projectile.SetActive(true);
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
