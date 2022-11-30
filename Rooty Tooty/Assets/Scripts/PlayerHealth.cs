// PlayerHealth.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private static int currentLife = 5;
    [SerializeField] private int maxLife = 5;

    public Animator animator;
    private bool dead;
    public GameObject gotHitScreen; //reference to the damage screen

    //public int tempPoint = 1; //temp var to place in gameoverscreen

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

    public void SetLife(int h) 
    {
        //currentLife must be a minimum of 1
        if (h <= 0)
            currentLife = 1;
        //currentLife cannot excede maxLife
        else if (h > maxLife)
            currentLife = maxLife;
        else 
            currentLife = h;

        HealthDisplay.instance.drawHeart(currentLife, maxLife);
    }

    public bool isDead()
    {
        return dead;
    }
    public void TakeDamage(int d)
    {
        if (currentLife > 0)
        {
            animator.SetTrigger("Damage");
            currentLife -= d;
            HealthDisplay.instance.drawHeart(currentLife, maxLife);
            gotHurtUI.instance.gotHurt(); // apply the gothitscreen
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
        Destroy(gameObject);
    }
}