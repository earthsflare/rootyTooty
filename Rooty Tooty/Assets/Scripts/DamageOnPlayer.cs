using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    private static bool canTakeDamage = true;
    [SerializeField] private int time = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            Player.instance.health.TakeDamage(1);
            Player.instance.health.knockBack(Player.instance.transform.position, true);
            StartCoroutine(damageTimer(time));
        }
    }
    // PlayerSingletonManager.instance.health
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.gameObject.CompareTag("Player"))
        {
            Player.instance.health.TakeDamage(1);
            Player.instance.health.knockBack(Player.instance.transform.position, true);
            StartCoroutine(damageTimer(time));
        }
    }

    private static IEnumerator damageTimer(int t)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(t);
        canTakeDamage = true;
    }
}