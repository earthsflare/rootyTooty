using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int life;
    public int maxLife = 3;

    void Start()
    {
        life = maxLife;
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}