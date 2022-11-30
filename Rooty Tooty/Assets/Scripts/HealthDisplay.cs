using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance { get; private set; }

    [SerializeField] private GameObject heartPrefab;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            //GameObject p = Instantiate(heartPrefab);
        }
    }

    public void drawHeart(int hearts, int maxHearts)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 1; i <= maxHearts; i++)
        {
            if(i <= hearts)
            {
                GameObject heart = Instantiate(heartPrefab, transform.position, Quaternion.identity);
                //heart.transform.parent = transform;
                heart.transform.SetParent(transform, false);
            }
        }
    }
}