using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && Player.instance.Jump.getJump() == 1)
        {
            Player.instance.Jump.enableDoubleJump();
            Destroy(gameObject);
            Debug.Log("Double jump unlocked!");
        }
    }
}