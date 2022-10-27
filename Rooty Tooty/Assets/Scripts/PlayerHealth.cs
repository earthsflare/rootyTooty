using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private int maxLife;
    private bool dead;
    public Animator animator;

    public GameObject gotHitScreen; //reference to the damage screen

    //public int tempPoint = 1; //temp var to place in gameoverscreen

    

    void Start()
    {
        life = hearts.Length;
        maxLife = life;
    }

    void Update()
    {
        /*
        if (dead == true)
        {
            StartCoroutine(deathAnim());
        }
        */
        if (gotHitScreen != null)
        {
            if (gotHitScreen.GetComponent<Image>().color.a > 0)
            {
                var color = gotHitScreen.GetComponent<Image>().color; // set variable color to color of image
                color.a -= 0.001f; // reduce alpha by 0.01 until it reaches 0 

                gotHitScreen.GetComponent<Image>().color = color;
            }
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
            gotHurt(); // apply the gothitscreen
            if (life < 1)
            {
                dead = true;
                MenuManager.instance.Setup();
                StartCoroutine(deathAnim());
            }
        }
    }
    void gotHurt()
    {
        var color = gotHitScreen.GetComponent<Image>().color; // set variable color to color of image   
        color.a = 0.5f; // change that color to show

        gotHitScreen.GetComponent<Image>().color = color; //assign it back to the image

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
        //gameManagerScript.instance.Setup(tempPoint); // displays the gameoverscreen with maxLife as the point display
        animator.SetBool("Dead", dead);
        yield return new WaitForSeconds(1.09f);
        Debug.Log("Player is dead!");
        //Delete player
    }

}