using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Camera mainCam;

    public Transform firePoint;
    public GameObject projectileToFire;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Input.mousePosition;

        Vector2 screenPoint = mainCam.WorldToScreenPoint(transform.localPosition);

        Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f,0f, angle);

        if(Input.GetMouseButtonDown(0))
        {
            // Instantiate(projectileToFire, firePoint.position, transform.rotation);
            GameObject projectile = ProjectilePooler.playerProjectilePool.GetPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
                projectile.SetActive(true);
            }
        }
    }
}
