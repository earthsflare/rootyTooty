using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public Rigidbody2D projectileRB;
    public int knockBackTime = 0;
    public float projectileLifespan = 2f;

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = transform.right * speed;
    }

    // Called when PlayerProjectile is SetActive(true)
    void OnEnable()
    {
        StartCoroutine(ProjectileTimeout());
    }

    // Coroutine for projectile timeout
    private IEnumerator ProjectileTimeout()
    {
        yield return new WaitForSeconds(projectileLifespan);
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
}
