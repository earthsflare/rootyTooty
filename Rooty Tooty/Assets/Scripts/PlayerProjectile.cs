using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage = 1; // Placeholder
    public Rigidbody2D projectileRB;
    public Animator projectileAnimator;
    public float offsetTime = 2f;
    public float projectileLifespan = 2f;
    private float speed = 10f;

    public void SetParent(GameObject newParent)
    {
        transform.parent = newParent.transform;
    }

    public void DetachFromParent()
    {
        transform.parent = null;
    }

    void Start()
    {
        projectileAnimator = GetComponent<Animator>();
    }

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

    // Called when PlayerProjectile is SetActive(false)
    void OnDisable()
    {
        speed = 10f;
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {       
        // Destroy(gameObject);
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("PlayerProjectile collided with " + collider.name);
            collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            StartCoroutine(ImpactAnimation());
        }
    }

    // Coroutine for impact animation
    private IEnumerator ImpactAnimation()
    {
        speed = 0;
        projectileAnimator.Play("Firebolt_Impact");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
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
