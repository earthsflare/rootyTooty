using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] private int damage = 1; // Placeholder
    //  [SerializeField] public float offsetTime = 2f;
    [SerializeField] public float projectileLifespan = 2f;
    [SerializeField] private float maxSpeed = 10f;

    [Header("Object References")]
    [SerializeField] private Rigidbody2D projectileRB;
    [SerializeField] private Animator projectileAnimator;
    [SerializeField] private Collider2D projectileCollider;

    private float currentSpeed = 10f;

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
        if(projectileAnimator == null)
            projectileAnimator = GetComponent<Animator>();
        if(projectileRB == null)
            projectileRB = GetComponent<Rigidbody2D>();
        if(projectileCollider == null)
            projectileCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * currentSpeed;
    }

    // Called when PlayerProjectile is SetActive(true)
    void OnEnable()
    {
        StartCoroutine(ProjectileTimeout());
    }

    // Called when PlayerProjectile is SetActive(false)
    void OnDisable()
    {
        currentSpeed = maxSpeed;
        projectileCollider.enabled = true;
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {       
        // Destroy(gameObject);
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("PlayerProjectile collided with Enemy: " + collider.name);
            projectileCollider.enabled = false;
            collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            StartCoroutine(ImpactAnimation());
        }
        else if (collider.CompareTag("Ground"))
        {
            Debug.Log("PlayerProjectile collided with Ground: " + collider.name);
            projectileCollider.enabled = false;
            StartCoroutine(ImpactAnimation());
        }
    }

    // Coroutine for impact animation
    private IEnumerator ImpactAnimation()
    {
        currentSpeed = 0;
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
