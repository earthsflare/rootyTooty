using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRoll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PickUpRoll") && Player.instance.roll.getRoll() == false)
        {
            Player.instance.roll.enableRoll();
            Destroy(collider.gameObject);
            Debug.Log("Roll unlocked!");
        }
    }
}