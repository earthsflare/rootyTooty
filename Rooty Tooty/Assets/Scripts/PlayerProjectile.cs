using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int damage = 1; // Placeholder
    [SerializeField] private float offsetTime = 5f;
    
    [Header("Object References")]
    [SerializeField] private Rigidbody2D projectileRB;
    [SerializeField] private  Animator projectileAnimator;

    private float currentSpeed = 10f;
    private float timer = 0f;

    public void SetParent(GameObject newParent)
    {
        transform.parent = newParent.transform;
    }

    public void DetachFromParent()
    {
        transform.parent = null;
    }

    private void OnEnable()
    {
        //Makes sure bulletspeed is not 0
        currentSpeed = projectileSpeed;
    }
    private void OnDisable()
    {
        //Prevents Coroutines to continue playing after bullet is "destroyed"
        StopAllCoroutines();
    }
    void Start()
    {
        if(projectileAnimator == null)
            projectileAnimator = GetComponent<Animator>();
        if(projectileRB == null)
            projectileRB = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * currentSpeed;

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

        // Destroy(gameObject);
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Debug.Log("PlayerProjectile collided with " + collider.name);
            StartCoroutine(ImpactAnimation());
        }
        else if (collider.CompareTag("Ground"))
        {
            Debug.Log("PlayerProjectile collided with " + collider.name);
            StartCoroutine(ImpactAnimation());
        }
    }

    // Coroutine for impact animation
    private IEnumerator ImpactAnimation()
    {
        currentSpeed = 0;
        projectileAnimator.Play("Firebolt_Impact");
        yield return new WaitForSeconds(0.5f);
        currentSpeed = projectileSpeed;
        gameObject.SetActive(false);
    }
}
