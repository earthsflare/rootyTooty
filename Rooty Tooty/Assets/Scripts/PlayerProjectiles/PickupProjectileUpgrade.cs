using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProjectileUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Player.instance.aim.upgradeWeapon();
            Debug.Log("Weapon upgraded!");
        }
    }
}
