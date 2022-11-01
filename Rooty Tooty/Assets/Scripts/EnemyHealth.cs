using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyHealth : MonoBehaviour
{
    public int life;
    public int maxLife = 3;
    public Animator enemyAnimator;

    void Start()
    {
        life = maxLife;
    }
    IEnumerator death()
    {
        gameObject.Find("gameobject").GetComponent("Enemy Health").enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

        public void TakeDamage(int damage)
    {
        enemyAnimator.Play("export_hit");
        life -= damage;
        if (life <= 0)
        {
            enemyAnimator.Play("export_death");
            StartCoroutine(death());
        }
    }
}