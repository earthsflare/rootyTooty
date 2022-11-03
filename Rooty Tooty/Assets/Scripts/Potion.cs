using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Potion") && Player.instance.health.getHealth() < Player.instance.health.getMaxHealth() && Player.instance.health.isDead() == false)
        {
            Player.instance.health.AddLife(1);
            Destroy(collider.gameObject);
        }
    }
}