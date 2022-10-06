using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private int maxLife;
    private bool dead;
    public Animator animator;

    void Start()
    {
        life = hearts.Length;
        maxLife = life;
    }

    void Update()
    {
        if (dead == true)
        {
            animator.SetBool("Dead", true);
            Destroy(gameObject);
            Debug.Log("Player is dead!");
            // game over screen overlay
        }
        animator.SetBool("Dead", false);

    }

    public int getHealth()
    {
        return life;
    }

    public int getMaxHealth()
    {
        return maxLife;
    }

    public bool isDead()
    {
        return dead;
    }

    public void TakeDamage(int d)
    {
        if (life >= 1)
        {
            animator.SetBool("Damage", true);
            life -= d;
            hearts[life].gameObject.SetActive(false);
            if (life < 1)
            {
                dead = true;
            }
        }
        animator.SetBool("Damage", false);

    }

    public void AddLife()
    {
        if (life < maxLife && dead == false)
        {
            hearts[life].gameObject.SetActive(true);
            life += 1;
        }
    }
}