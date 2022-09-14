using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private bool dead;

    private void Start()
    {
        life = hearts.Length;
    }

    void Update()
    {
        if(dead == true)
        {
            Destroy(gameObject);
            Debug.Log("Player is dead!");
            // game over screen overlay
        }
    }

    public void TakeDamage(int d)
    {
        if (life > 0)
        {
            life -= d;
            Destroy(hearts[life].gameObject);
            if (life < 1)
            {
                dead = true;
            }
        }
    }
}
