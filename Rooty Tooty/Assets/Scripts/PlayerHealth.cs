// PlayerHealth.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int currentLife = 5;
    [SerializeField] private int maxLife = 5;

    public Animator animator;
    private bool dead;
    public GameObject gotHitScreen; //reference to the damage screen

    //public int tempPoint = 1; //temp var to place in gameoverscreen

    [SerializeField] private float decreaseAlpha;
    [SerializeField] private float initialAlpha;

    void Start()
    {
        currentLife = maxLife;
        dead = false;
        HealthDisplay.instance.drawHeart(currentLife, maxLife);
    }

    public int getHealth()
    {
        return currentLife;
    }

    public int getMaxHealth()
    {
        return maxLife;
    }

    public bool isDead()
    {
        return dead;
    }

    public void Update()
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
                color.a -= decreaseAlpha; // reduce alpha by 0.01 until it reaches 0 

                gotHitScreen.GetComponent<Image>().color = color;
            }
        }
    }

    public void gotHurt()
    {
        var color = gotHitScreen.GetComponent<Image>().color; // set variable color to color of image   
        color.a = initialAlpha; // change that color to show

        gotHitScreen.GetComponent<Image>().color = color; //assign it back to the image

    }

    public void TakeDamage(int d)
    {
        if (currentLife > 0)
        {
            animator.SetTrigger("Damage");
            currentLife -= d;
            HealthDisplay.instance.drawHeart(currentLife, maxLife);
            gotHurt(); // apply the gothitscreen
            if (currentLife < 1)
            {
                dead = true;
                MenuManager.instance.Setup();
                StartCoroutine(deathAnim());
            }
        }
    }
    
    public void AddLife(int h)
    {
        if (currentLife < maxLife && isDead() == false)
        {
            currentLife += h;
            HealthDisplay.instance.drawHeart(currentLife, maxLife);
            
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