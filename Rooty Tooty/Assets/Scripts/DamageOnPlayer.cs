using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    private static bool canTakeDamage = true;
    private static int wait = 0;
    [SerializeField] private int time = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Player") && wait <= 0)
        {
            wait = wait + 1;
            Player.instance.health.TakeDamage(1);
            StartCoroutine(damageTimer());
            wait = wait - 1;
        }
    }
    // PlayerSingletonManager.instance.health
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.gameObject.CompareTag("Player"))
        {
            wait = wait + 1;
            Player.instance.health.TakeDamage(1);
            StartCoroutine(damageTimer());
            wait = wait - 1;
        }
    }

    private IEnumerator damageTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(time);
        canTakeDamage = true;
    }
}