using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private int maxLife;
    private bool dead;

    void Start()
    {
        life = hearts.Length;
        maxLife = life;
    }

    void Update()
    {
        if (dead == true)
        {
            Destroy(gameObject);
            Debug.Log("Player is dead!");
            // game over screen overlay
        }
    }

    public void TakeDamage(int d)
    {
        if (life >= 1)
        {
            life -= d;
            hearts[life].gameObject.SetActive(false);
            if (life < 1)
            {
                dead = true;
            }
        }
    }

    public void AddLife()
    {
        if (life < maxLife && dead == false)
        {
            hearts[life].gameObject.SetActive(true);
            life += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Potion") && life < maxLife && dead == false)
        {
            AddLife();
            Destroy(collider.gameObject);
        }
    }
}