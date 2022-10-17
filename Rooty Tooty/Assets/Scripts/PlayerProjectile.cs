using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1; // Placeholder
    public Rigidbody2D projectileRB;
    public Animator projectileAnimator;
    public float offsetTime = 2f;
    private float timer = 0f;

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
            collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            Debug.Log("PlayerProjectile collided with " + collider.name);
            StartCoroutine(ImpactAnimation());
        }
    }

    // Coroutine for impact animation
    private IEnumerator ImpactAnimation()
    {
        speed = 0;
        projectileAnimator.Play("Firebolt_Impact");
        yield return new WaitForSeconds(0.5f);
        speed = 10f;
        gameObject.SetActive(false);
    }
}
