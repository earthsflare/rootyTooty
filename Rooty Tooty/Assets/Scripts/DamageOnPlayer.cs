using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    private static bool canTakeDamage = true;
    [SerializeField] private int time = 1;

    private float timer = 0;

    private void Update()
    {
        if (timer <= 0)
            return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 0;
            canTakeDamage = true;

            Player.instance.move.canMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            canTakeDamage = false;
            timer += time;
            Player.instance.health.TakeDamage(1);
            Player.instance.move.canMove = false;
            Player.instance.health.knockBack(Player.instance.move.Enemy.transform.position, Player.instance.transform.position, Player.instance.move.rb, true);
            //StartCoroutine(damageTimer(time));
        }
    }

    // PlayerSingletonManager.instance.health
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.gameObject.CompareTag("Player"))
        {
            canTakeDamage = false;
            timer += time;

            Player.instance.health.TakeDamage(1);
            Player.instance.move.canMove = false;
            Player.instance.health.knockBack(Player.instance.move.Enemy.transform.position, Player.instance.transform.position, Player.instance.move.rb, true);
            //StartCoroutine(damageTimer(time));
        }
    }

    /*private IEnumerator damageTimer(int t)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(t);
        canTakeDamage = true;
        Player.instance.move.canMove = true;
    }
    */
}