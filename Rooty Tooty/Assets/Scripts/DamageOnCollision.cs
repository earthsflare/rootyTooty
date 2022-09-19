using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    private bool canTakeDamage = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(1);
            StartCoroutine(damageTimer());
        }
    }

    private IEnumerator damageTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }
}
