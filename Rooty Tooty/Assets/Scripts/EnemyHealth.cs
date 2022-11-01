using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int life;
    public int maxLife = 3;
    public Animator enemyAnimator;

    void Start()
    {
        life = maxLife;
    }

    public void TakeDamage(int damage)
    {
        GetComponent<Animation>().Play("ogre warrior - export_hit");
        life -= damage;
        if (life <= 0)
        {
            GetComponent<Animation>().Play("ogre warrior -export_death");
            Destroy(gameObject);
        }
    }
}