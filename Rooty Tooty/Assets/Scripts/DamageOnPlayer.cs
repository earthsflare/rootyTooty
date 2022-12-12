using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnPlayer : MonoBehaviour
{
    [SerializeField] protected int damage = 1;
    protected static bool canHurtPlayer = true;
    [SerializeField] protected float time = 3;
    [SerializeField] protected bool canKnock = true;
    public int knockBackTime = 3;
    [SerializeField] protected Transform knockbackCenter = null;

    protected float timer = 0;

    protected void Awake()
    {
        if (knockbackCenter == null)
            knockbackCenter = transform;
    }

    protected void OnDisable()
    {
        if (timer != 0 && !canHurtPlayer)
            canHurtPlayer = true;
    }

    protected void FixedUpdate()
    {
        if (timer <= 0)
            return;
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            timer = 0;
            canHurtPlayer = true;

            //Player.instance.Move.canMove = true;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (canHurtPlayer && collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {

        if (canHurtPlayer && collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    // PlayerSingletonManager.instance.health
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (canHurtPlayer && collider.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    protected void AttackPlayer()
    {
        Debug.Log("Attacking Player");
        canHurtPlayer = false;
        timer = time;
        Player.instance.Health.TakeDamage(damage);
        //Player.instance.Move.canMove = false;
        //Player.instance.Move.knockBack(gameObject.transform.position, Player.instance.transform.position, Player.instance.Move.rb, canKnock, knockBackTime);
    }
}