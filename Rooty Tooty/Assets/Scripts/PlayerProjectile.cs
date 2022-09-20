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

    private void OnCollisionEnter2D (Collision2D collision)
    {
        // Add animation for impact later
        // Instantiate(impactAnimation, transform.position, transform.rotation);

        // Destroy(gameObject);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
