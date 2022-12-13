using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    static int counter = 9;
    const int check = 7;
    const int reset = 10;

    public void resetCounter()
    {
        counter = 0;
    }

    public void addCounter(Vector3 coord)
    {
        int tmp = counter;
        if(counter < check)
        {
            counter += 1;
        }

        if(tmp == counter && counter >= check && Player.instance.Health.getHealth() < Player.instance.Health.getMaxHealth())
        {
            counter += 1;
        }
        
        if(counter == reset)
        {
            GameObject drop = HealthPickupPool.instance.GetPooledObject();
            if(drop != null)
            {
                drop.transform.position = coord;
                drop.SetActive(true);
            }
            resetCounter();
        }
    }
}
