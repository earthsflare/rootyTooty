using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class EnemyHealth : MonoBehaviour
{
    public int life;
    public int maxLife = 3;
    public Animator enemyAnimator;
    public float deathTime = 1.5f;
    void Start()
    {
        life = maxLife;
    }

    public void TakeDamage(int damage)
    {
        enemyAnimator.Play("export_hit");
        life -= damage;
        if (life <= 0)
        {
            StartCoroutine(Death());
        }
    }
    private IEnumerator Death()
    {
        enemyAnimator.Play("export_death");
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}