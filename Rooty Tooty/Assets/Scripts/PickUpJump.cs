using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PickUpJump") && Player.instance.jump.getJump() == 1)
        {
            Player.instance.jump.enableDoubleJump();
            Destroy(collider.gameObject);
            Debug.Log("Double jump unlocked!");
        }
    }
}