using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; // Placeholder
    public Rigidbody2D projectileRB;
    public GameObject impactAnimation;
    public float offsetTime = 2f;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * speed;

        // Deactivate projectile after offsetTime seconds
        // Not sure how efficient this is?
        // Destroy(gameObject, offsetTime) might be better, but don't want to destroy
        timer += Time.deltaTime;
        if(timer > offsetTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }
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
