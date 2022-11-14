using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    protected static bool canTakeDamage = true;
    [SerializeField] protected float time = 1;
    [SerializeField] protected bool canKnock = true;
    public int knockBackTime = 3;
    [SerializeField] protected Transform knockbackCenter = null;

    protected float timer = 0;

    protected void Awake()
    {
        if (knockbackCenter == null)
            knockbackCenter = transform;
    }

    protected void Update()
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

    protected void OnCollisionEnter2D(Collision2D collision)
    {

        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            canTakeDamage = false;
            timer += time;
            Player.instance.health.TakeDamage(damage);
            Player.instance.move.knockBack(gameObject.transform.position, Player.instance.transform.position, Player.instance.move.rb, canKnock, knockBackTime);
            //StartCoroutine(damageTimer(time));
        }
    }

    // PlayerSingletonManager.instance.health
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.gameObject.CompareTag("Player"))
        {

            canTakeDamage = false;
            timer += time;

            Player.instance.health.TakeDamage(damage);
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