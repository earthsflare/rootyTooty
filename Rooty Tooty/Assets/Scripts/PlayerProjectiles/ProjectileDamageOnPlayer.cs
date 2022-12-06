using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageOnPlayer : DamageOnPlayer
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {

        base.OnCollisionStay2D(collision);
        if (canTakeDamage && collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if (canTakeDamage && collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
