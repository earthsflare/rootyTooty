using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public Rigidbody2D projectileRB;
    public float projectileLifespan = 2f;

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * speed;
    }

    // Called when PlayerProjectile is SetActive(true)
    void OnEnable()
    {
        StartCoroutine(ProjectileTimeout());
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        // Add animation for impact later
        // Instantiate(impactAnimation, transform.position, transform.rotation);

        if (collider.CompareTag("Player"))
        {
            Debug.Log("EnemyProjectile collided with " + collider.name);
            collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            Player.instance.health.knockBack(transform.position, Player.instance.transform.position, Player.instance.move.rb, false);
            gameObject.SetActive(false);
        }
    }

    // Coroutine for projectile timeout
    private IEnumerator ProjectileTimeout()
    {
        yield return new WaitForSeconds(projectileLifespan);
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
}
