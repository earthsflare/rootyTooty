using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private AudioSource Potionsfx;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && Player.instance.Health.getHealth() < Player.instance.Health.getMaxHealth() && Player.instance.Health.isDead() == false)
        {
            Player.instance.Health.AddLife(1);
            Potionsfx.Play();
            Destroy(gameObject);
        }
    }
}