using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageOnPlayer : DamageOnPlayer
{
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (canTakeDamage && collider.CompareTag("Player"))
        {
            Debug.Log("EnemyProjectile collided with " + collider.name);
            collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            Player.instance.move.knockBack(transform.position, Player.instance.transform.position, Player.instance.move.rb, false, knockBackTime);
            gameObject.SetActive(false);
        }
        base.OnTriggerEnter2D(collider);
    }
}
