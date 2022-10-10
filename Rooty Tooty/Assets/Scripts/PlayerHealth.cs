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

    public int tempPoint = 1; //temp var to place in gameoverscreen

    //public GameOverScreen GameOverScreen;
    public gameManagerScript GameManagerScript; // for displaying the gameover screen

    void Start()
    {
        life = hearts.Length;
        maxLife = life;
    }

    void Update()
    {
        if (dead == true)
        {
            StartCoroutine(deathAnim());
        }
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
            animator.SetTrigger("Damage");
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

    private IEnumerator deathAnim()
    {
        animator.SetBool("Dead", dead);
        yield return new WaitForSeconds(1.09f);
        Destroy(gameObject);
        Debug.Log("Player is dead!");
        GameManagerScript.Setup(tempPoint); // displays the gameoverscreen with maxLife as the point display

    }

    public void GameOver()
    {

    }
}