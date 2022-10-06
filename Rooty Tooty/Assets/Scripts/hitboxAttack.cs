using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxAttack : MonoBehaviour
{
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
