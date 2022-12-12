using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWallJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.instance.ToggleWallJump(true);
            Destroy(gameObject);
            Debug.Log("Double jump unlocked!");
        }
    }
}