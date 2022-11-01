using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    private static bool canTakeDamage = true;
    [SerializeField] private float time = 1;
    [SerializeField] private bool canKnock = true;
    public int knockBackTime = 3;
    [SerializeField] private Transform knockbackCenter = null;

    private float timer = 0;

    private void Awake()
    {
        if (knockbackCenter == null)
            knockbackCenter = transform;
    }

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
            Player.instance.move.knockBack(gameObject.transform.position, Player.instance.transform.position, Player.instance.move.rb, canKnock, knockBackTime);
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
            Player.instance.move.knockBack(gameObject.transform.position, Player.instance.transform.position, Player.instance.move.rb, canKnock, knockBackTime);
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