using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnEnemy : MonoBehaviour
{
    private bool canTakeDamage = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            // StartCoroutine(damageTimer());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.gameObject.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
            // StartCoroutine(damageTimer());
        }
    }

    /*
    private IEnumerator damageTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }
    */
}