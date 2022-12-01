using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageOnPlayer : DamageOnPlayer
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        base.OnCollisionStay2D(collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        base.OnTriggerEnter2D(collider);
    }
}
