using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRoll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PickUpRoll") && Player.instance.move.getRoll() == false)
        {
            Player.instance.move.enableRoll();
            Destroy(collider.gameObject);
            Debug.Log("Roll unlocked!");
        }
    }
}