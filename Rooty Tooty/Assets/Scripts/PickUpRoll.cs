using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRoll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && Player.instance.Roll.getRoll() == false)
        {
            Player.instance.ToggleRoll(true);
            Destroy(gameObject);
            Debug.Log("Roll unlocked!");
        }
    }
}