using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; // Placeholder
    public Rigidbody2D projectileRB;
    public GameObject impactAnimation;

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        // Add animation for impact later
        // Instantiate(impactAnimation, transform.position, transform.rotation);

        // Destroy(gameObject);
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("PlayerProjectile collided with " + collider.name);
            gameObject.SetActive(false);
        }
    }
}
